﻿using EPGManager.Interfaces;
using EPGManager.Models;
using System;
using System.Collections.Generic;

namespace EPGManager
{
    public class EPGRepository : IEPGRepository
    {
        public List<Channel> ListChannels(int currentPage, int PageSize, bool underage)
        {
            var channels = new List<Channel>();

            if(currentPage <= 3)              
            for (int i = 0 ; i < PageSize; i++)
            {
                channels.Add(new Channel
                {
                    Id = $"Zoomoo.br",
                    Name = $"Zoomoo BR{i}",
                    Icon = "https://static.epg.best/br/Zoomoo.br.png"
                });
            }

            return channels;
        }

        public List<Programme> ListProgramme(bool underage)
        {
            var programmes = new List<Programme>();
            for (int i = 0; i < 50 ; i++)
            {
                programmes.Add(new Programme
                {
                    Start = DateTime.Parse("20210326000500 +0000"),
                    Stop = DateTime.Parse("20210326005500 +0000"),
                    ChannelId = "Zoomoo.br",
                    Title = "CSI",
                    SubTitle = "Split Decisions",
                    Description = "Quando a equipe CSI é chamada para investigar um homem que é baleado à queima-roupa em um cassino local, eles fecham o prédio inteiro para encontrar seu assassino",
                    Category = new string[] { "Séries", "Ação" },
                    Icon = "https://cdn.mitvstatic.com/programs/br_csi-s12e19-split-decisions_l_m.jpg"
                });
            }

            return programmes;
        }
    }
}