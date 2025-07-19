// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;

UpdateSelf a = new UpdateSelf();

public class UpdateSelf
{
    string LastVersion = "";
    string InstalledVersion = "";
    string DownloadUrl = "";
    public static Dictionary<string, string> ConfigInfo = new Dictionary<string, string>();
    public static string JsonConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
    public UpdateSelf()
    {
        ConfigInfo = JsonConvert.DeserializeObject<Dictionary<string, string?>>(File.ReadAllText(JsonConfigPath)) ?? ConfigInfo;
        InstalledVersion = ConfigInfo["version"];
        try
        {
            GetNewVersion();
        }
        catch(Exception ex) { Console.WriteLine("升级时出现异常~可能网络有问题");Console.WriteLine(ex.Message); }
        string startPath = Path.Combine(Path.GetFullPath("../.."), ConfigInfo["name"], ConfigInfo["exename"]);
        if (Path.Exists(startPath))
        {
            Process.Start(startPath, LastVersion);
        }
    }

    void DownloadFiles()
    {
        using (var client = new HttpClient())
        {
            if (DownloadUrl == "") return;
            string tmpFilePath = Path.Combine(Path.GetFullPath("..//.."), "update", $"{ConfigInfo["name"]}.{LastVersion}.zip");  //下载文件名
            string tmpDirctory = Path.GetFullPath("..", tmpFilePath);  //下载路径
            //string bakFilePath = Path.Combine(tmpDirctory, "update", $"config.json");  //备份文件名
            string desFilePath = Path.Combine(Path.GetFullPath("..", tmpDirctory), ConfigInfo["name"]);  //目标路径

            if (!Path.Exists(tmpDirctory))
            {
                Directory.CreateDirectory(tmpDirctory);
            }
            try
            {
                HttpResponseMessage response = client.GetAsync(DownloadUrl).Result;
                var a = response.EnsureSuccessStatusCode();
                byte[] content = response.Content.ReadAsByteArrayAsync().Result;
                File.WriteAllBytes(tmpFilePath, content);

                //备份配置文件
                if (Path.Exists(Path.Combine(desFilePath, "config.json")))
                { File.Copy(Path.Combine(desFilePath, "config.json"), Path.Combine(tmpDirctory, "config.json"), true); }

                //覆盖文件
                ZipFile.ExtractToDirectory(tmpFilePath, desFilePath, true);

                //还原配置文件
                if (Path.Exists(Path.Combine(tmpDirctory, "config.json")))
                { File.Copy(Path.Combine(tmpDirctory, "config.json"), Path.Combine(desFilePath, "config.json"), true); }

                // 更新本地版信息
                Task.Run(() =>
                {
                    ConfigInfo["version"] = LastVersion;
                    File.WriteAllTextAsync(JsonConfigPath, JsonConvert.SerializeObject(ConfigInfo, Formatting.Indented));
                });
                Console.WriteLine("更新成功!");
            }
            catch (Exception ex) { Console.WriteLine($"更新失败!\r\n{ex.Message}"); }
        }
    }



    public int CompareVersion(string oldVersion = null, string newVersion = null)
    {
        var o = (oldVersion ?? InstalledVersion).Split('.').Select(int.Parse).ToArray();
        var n = (newVersion ?? LastVersion).Split('.').Select(int.Parse).ToArray();
        for (int i = 0; i < o.Length || i < n.Length; i++)
        {
            int i1 = i < o.Length ? o[i] : 0;
            int i2 = i < n.Length ? n[i] : 0;
            if (i1 > i2) return -1;
            if (i2 > i1) return 1;
        }
        return 0;
    }
    void GetNewVersion(string url = "")
    {
        using (HttpClient client = new HttpClient())
        {
            // 设置请求的URL
            url = url != "" ? url : ConfigInfo["updateurl"];
            url += $"?name={ConfigInfo["name"]}";
            try
            {
                // 发送GET请求
                HttpResponseMessage response = client.GetAsync(url).Result;

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseBody = response.Content.ReadAsStringAsync().Result;

                // 输出响应内容
                var ret = JsonConvert.DeserializeObject<Dictionary<string, object?>>(responseBody);
                if (Convert.ToString(ret["code"]) == "200")
                {
                    Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(Convert.ToString(ret["data"]));
                    LastVersion = Convert.ToString(data["version"] ?? "0.0");
                    DownloadUrl = Convert.ToString(data["url"] ?? "");

                    switch (CompareVersion())
                    {
                        case 0:
                            break;
                        case 1:
                            DownloadFiles();
                            break;
                        case -1: break;

                    };
                }
            }
            catch (HttpRequestException e)
            {
                // 处理请求异常
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}