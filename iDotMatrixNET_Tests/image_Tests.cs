using iDotMatrixNET;
using iDotMatrixNET.Classes;
using Windows.System;

namespace iDotMatrixNET_Tests
{
    [TestClass]
    public class image_Tests
    {
        [TestMethod]
        public async Task PngCreatePayloads_ShoudBe_Ok()
        {
            var image = new Image();
            var bytes = image.LoadPng("C:\\Users\\Luis\\Desktop\\test.png");

            var payloads = image.CreatePayloads(bytes);
            
        }


        [TestMethod]
        public async Task GifCreatePayloads_ShoudBe_Ok()
        {
            var gif = new Gif();
            var bytes = gif.LoadGif("C:\\Users\\Luis\\Desktop\\10.gif");

            var payloads = gif.CreatePayloads(bytes);

        }
    }
}