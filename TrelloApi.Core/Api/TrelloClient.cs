using Newtonsoft.Json;
using RestSharp;
using TrelloApi.Core.Models;
using TrelloApi.Core.Utils;
namespace TrelloApi.Core.Api
{
    public class TrelloClient
    {
        private readonly RestClient _client;
        private readonly Settings _settings;
        public TrelloClient(Settings settings)
        {
            _settings = settings;
            _client = new RestClient(settings.BaseUrl);
        }
        private RestRequest Create(string resource, Method method)
        {
            var req = new RestRequest(resource, method);
            req.AddQueryParameter("key", _settings.ApiKey);
            req.AddQueryParameter("token", _settings.ApiToken);
            return req;
        }
        // Boards
        public async Task<Board> CreateBoardAsync(string name)
        {
            var req = Create("/boards", Method.Post);
            req.AddQueryParameter("name", name);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "CreateBoard");
            return JsonConvert.DeserializeObject<Board>(resp.Content!)!;
        }
        public async Task DeleteBoardAsync(string boardId)
        {
            var req = Create($"/boards/{boardId}", Method.Delete);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "DeleteBoard");
        }
        // Lists
        public async Task<List> CreateListAsync(string boardId, string
        listName)
        {
            var req = Create("/lists", Method.Post);
            req.AddQueryParameter("name", listName);
            req.AddQueryParameter("idBoard", boardId);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "CreateList");
            return JsonConvert.DeserializeObject<List>(resp.Content!)!;
        }
        public async Task<List<List>> GetBoardListsAsync(string boardId)
        {
            var req = Create($"/boards/{boardId}/lists", Method.Get);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "GetBoardLists");
            return JsonConvert.DeserializeObject<List<List>>(resp.Content!)!;
        }
        // Cards
        public async Task<Card> CreateCardAsync(string idList, string
        cardName)
        {
            var req = Create("/cards", Method.Post);
            req.AddQueryParameter("idList", idList);
            req.AddQueryParameter("name", cardName);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "CreateCard");
            return JsonConvert.DeserializeObject<Card>(resp.Content!)!;
        }
        public async Task<Card> UpdateCardNameAsync(string cardId, string
        newName)
        {
            var req = Create($"/cards/{cardId}", Method.Put);
            req.AddQueryParameter("name", newName);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "UpdateCard");
            return JsonConvert.DeserializeObject<Card>(resp.Content!)!;
        }
        public async Task DeleteCardAsync(string cardId)
        {
            var req = Create($"/cards/{cardId}", Method.Delete);
            var resp = await _client.ExecuteAsync(req);
            EnsureSuccess(resp, "DeleteCard");
        }
        private static void EnsureSuccess(RestResponse resp, string action)
        {
            if (!resp.IsSuccessful)
            {
                var msg = $"Trello API hata: {action} -> Status: {resp.StatusCode}, Content: {resp.Content}";
                throw new ApplicationException(msg);
            }
        }
    }
}