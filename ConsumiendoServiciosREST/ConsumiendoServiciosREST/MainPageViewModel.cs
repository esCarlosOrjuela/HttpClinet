using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace ConsumiendoServiciosREST
{
    public class MainPageViewModel
    {

        public HttpClient HttpClient { get; set; }
        public JsonSerializerOptions JsonSerializerOption { get; set; }

        string baseURL = "https://62e1401cfa99731d75d247cc.mockapi.io";

        public MainPageViewModel()
        {
            HttpClient = new HttpClient();
            JsonSerializerOption = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public ICommand GetAllUsersCommand => new Command(async () =>
        {
            var url = $"{baseURL}/users";
            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<User>>(responseStream, JsonSerializerOption);
                }
            }
        });

        public ICommand GetSingleUserCommand => new Command(async () =>
        {
            var url = $"{baseURL}/users/25";
            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<User>(responseStream, JsonSerializerOption);
                }
            }
        });

        public ICommand AddUserCommand => new Command(async () =>
        {
            var newUser = new User
            {
                createdAt = DateTime.Now,
                name = "Carlos Orjuela",
                avatar = "https://fakeimg.pl/350x200/?text=MAUI"
            };

            string jsonUser = JsonSerializer.Serialize<User>(newUser, JsonSerializerOption);

            var url = $"{baseURL}/users";
            StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, content);

            /// to Update Data
            /// var response = await HttpClient.PutAsync(url, content);
            ///
            /// to Delete Data
            /// var response = await HttpClient.DeleteAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<User>(responseStream, JsonSerializerOption);
                }
            }
        });
    }
}


