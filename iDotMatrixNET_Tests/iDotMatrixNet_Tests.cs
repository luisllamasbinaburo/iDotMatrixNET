using iDotMatrixNET;
using iDotMatrixNET.Classes;

namespace iDotMatrixNET_Tests
{
    [TestClass]
    public class iDotMatrixNet_Tests
    {
        [TestMethod]
        public async Task Connect_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            await panel.Connect();                     
        }

        [TestMethod]
        public async Task TurnOn_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            var rst = await panel.Connect();

            await panel.TurnOn();
        }

        [TestMethod]
        public async Task TurnOff_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            var rst = await panel.Connect();

            await panel.TurnOff();
        }

        [TestMethod]
        public async Task SetPixel_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            var rst = await panel.Connect();

            var color = new Color(0x00, 0xFF, 0x00);

            await panel.SetPixel(0, color);
        }


        [TestMethod]
        public async Task SetPng_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            var rst = await panel.Connect();

            var imagePath = "C:\\Users\\Luis\\Desktop\\test.png";

            await panel.SetPng(imagePath);
        }

        [TestMethod]
        public async Task SetGif_ShoudBe_Ok()
        {
            var panel = new iDotMatrixNET.iDotMatrixNet("IDM-7325B1");
            var rst = await panel.Connect();

            var imagePath = "C:\\Users\\Luis\\Desktop\\10.gif";

            await panel.SetGif(imagePath);
        }

    }
}