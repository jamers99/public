using RideScheduler.Model;

namespace RideScheduler.Tests
{
    public class DataProviderTests : TestBase
    {
        [Test]
        public async Task Rider_WriteRead()
        {
            var rider = new Rider() { Username = "Test" };
            await DataProvider.CreateRiderAsync(rider, "pw");
            Assert.That(rider.HashedPassword, Is.Not.Null.Or.Empty, "Should be saving the hashed value");
            Assert.That(rider.HashedPassword, Is.Not.EqualTo("pw"), "Should be saving the hashed value");

            var nullRider = await DataProvider.GetRiderAsync("Yip", "pw");
            Assert.That(nullRider, Is.Null, "Username null");

            var yipRider = await DataProvider.GetRiderAsync("Yip", "pw");
            Assert.That(yipRider, Is.Null, "Username incorrect");

            var wrongPwRider = await DataProvider.GetRiderAsync("Test", "ying");
            Assert.That(wrongPwRider, Is.Null, "Password incorrect");

            var foundRider = await DataProvider.GetRiderAsync("Test", "pw");
            Assert.That(foundRider, Is.Not.Null, "Username and password correct");
        }
    }
}