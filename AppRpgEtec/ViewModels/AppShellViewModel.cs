﻿using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private static string conexaoAzureStorage = "";
        private static string container = "arquivos";

        public AppShellViewModel()
        {

            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);

            CarregarUsuarioAzure();

            
            
        }
        private byte[] foto;
        public byte[] Foto
        {
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged();
            }
        }

        public async void CarregarUsuarioAzure()
        {
            try
            {
                int usuarioId = Preferences.Get("UsuariosId", 0);
                string filename = $"{usuarioId}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, filename);
                Byte[] fileBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    blobClient.OpenRead().CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                Foto = fileBytes;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }



    }
}
