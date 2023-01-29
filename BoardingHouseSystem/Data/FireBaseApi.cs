using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Android.App;
using Android.Provider;
using System.Net;
using Java.Lang;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Java.Util;
using Firebase.Storage;

namespace BoardingHouseSystem
{
    public class FireBaseApi
    {
        public static async Task<string> UploadFile(string fileName, string filePath)
        {
            using (var ms = new MemoryStream(System.IO.File.ReadAllBytes(filePath)))
            {
                return  await new FirebaseStorage(Constants.FireBaseUrl, new FirebaseStorageOptions
                        {
                            ThrowOnCancel = true,
                        })
                        .Child(Constants.FireBaseFolder)
                        .Child(fileName)
                        .PutAsync(ms);
            }
        }
    }
}
