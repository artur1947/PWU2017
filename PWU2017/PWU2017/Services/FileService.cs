using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;

namespace PWU2017.Services
{
    public class FileService
    {
        public async Task SaveAsync(Models.FileInfo model)
        {
            if (model != null)
                await Windows.Storage.FileIO.WriteTextAsync(model.Ref, model.Text);
        }

        public async Task<Models.FileInfo> LoadAsync(Windows.Storage.StorageFile file)
        {
            // add to jumplist
            if (Windows.UI.StartScreen.JumpList.IsSupported())
            {
                var jlist = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
                jlist.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.None;
                while (jlist.Items.Count() > 4)
                    jlist.Items.RemoveAt(jlist.Items.Count() - 1);
                if (!jlist.Items.Any(x => x.Arguments == file.Path))
                {
                    var jitem = Windows.UI.StartScreen.JumpListItem.CreateWithArguments(file.Path, file.DisplayName);
                    jlist.Items.Add(jitem);
                }
                await jlist.SaveAsync();
            }

            // add to mru
            var mruList = StorageApplicationPermissions.MostRecentlyUsedList;
            while (mruList.Entries.Count() >= mruList.MaximumItemsAllowed)
                mruList.Remove(mruList.Entries.First().Token);
            if (!mruList.Entries.Any(x => x.Metadata == file.Path))
                mruList.Add(file, file.Path);
            var FutureAccessList = StorageApplicationPermissions.FutureAccessList;
            FutureAccessList.Add(file);

            //build model
            return new Models.FileInfo
            {
                Text = await Windows.Storage.FileIO.ReadTextAsync(file),
                Name = file.DisplayName,
                Ref = file,
            };
        }
    }
}
