using NUnit.Framework;
using TrelloApi.Core.Api;
using TrelloApi.Core.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace TrelloApi.Tests
{
    [TestFixture]
    public class TrelloE2ETests
    {
        private Settings _settings = null!;
        private TrelloClient _trello = null!;
        [SetUp]
        public void Setup()
        {
            _settings = Config.Load();
            _trello = new TrelloClient(_settings);
        }
        [Test]
        public async Task Trello_E2E_Board_Cards()
        {
            string boardName = _settings.BoardNamePrefix +
            DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string listName = _settings.DefaultListName;
            string boardId = string.Empty;
            string listId = string.Empty;
            string card1Id = string.Empty;
            string card2Id = string.Empty;
            try
            {
                // 1) Board oluştur
                var board = await _trello.CreateBoardAsync(boardName);
                Assert.That(board, Is.Not.Null);
                Assert.That(board.Id, Is.Not.Empty);
                boardId = board.Id;
                TestContext.WriteLine($"Board oluşturuldu: {board.Name} ({board.Id})");

                // 2) Liste oluştur
                var list = await _trello.CreateListAsync(boardId, listName);
                Assert.That(list, Is.Not.Null);
                Assert.That(list.Id, Is.Not.Empty);
                listId = list.Id;
                TestContext.WriteLine($"Liste oluşturuldu: {list.Name} ({list.Id})");

                // 3) İki kart ekle
                var card1 = await _trello.CreateCardAsync(listId, "Card #1");
                var card2 = await _trello.CreateCardAsync(listId, "Card #2");
                Assert.That(card1, Is.Not.Null);
                Assert.That(card2, Is.Not.Null);
                card1Id = card1.Id;
                card2Id = card2.Id;
                TestContext.WriteLine($"Kartlar oluşturuldu: {card1.Id}, {card2.Id}");

                // 4) Rastgele kartı güncelle
                var randomCard = new[] { card1, card2 }[new Random().Next(2)];
                var updatedName = "Updated Card";
                var updatedCard = await _trello.UpdateCardNameAsync(randomCard.Id, updatedName);
                Assert.That(updatedCard.Name, Is.EqualTo(updatedName), "Kart güncellemesi başarısız");
                TestContext.WriteLine($"Kart güncellendi: {updatedCard.Id} -> {updatedCard.Name}");

                // 5) Kartları sil
                await _trello.DeleteCardAsync(card1Id);
                await _trello.DeleteCardAsync(card2Id);
                TestContext.WriteLine("Kartlar silindi");

                // 6) Board’u sil
                await _trello.DeleteBoardAsync(boardId);
                TestContext.WriteLine("Board silindi");

            }
            catch (Exception ex)
            {
                Assert.Fail($"Test sırasında hata oluştu: {ex.Message}");
            }
        }
    }
}