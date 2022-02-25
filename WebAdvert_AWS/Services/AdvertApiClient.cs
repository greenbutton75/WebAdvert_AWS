﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AdvertAPI.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly string _baseAddress;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;

            _baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            var advertApiModel = _mapper.Map<AdvertModel>(model);

            var jsonModel = JsonSerializer.Serialize(advertApiModel, new JsonSerializerOptions( ));
            var response = await _client.PostAsync(new Uri($"{_baseAddress}/Create"),
                new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var createAdvertResponse = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>().ConfigureAwait(false);
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdvertModel>(model);
            var jsonModel = JsonSerializer.Serialize(advertModel);
            var response = await _client
                .PutAsync(new Uri($"{_baseAddress}/confirm"),
                    new StringContent(jsonModel, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/all")).ConfigureAwait(false);
            var allAdvertModels = await apiCallResponse.Content.ReadFromJsonAsync<List<AdvertModel>>().ConfigureAwait(false);
            return allAdvertModels.Select(x => _mapper.Map<Advertisement>(x)).ToList();
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/{advertId}")).ConfigureAwait(false);
            var fullAdvert = await apiCallResponse.Content.ReadFromJsonAsync<AdvertModel>().ConfigureAwait(false);
            return _mapper.Map<Advertisement>(fullAdvert);
        }
    }
}