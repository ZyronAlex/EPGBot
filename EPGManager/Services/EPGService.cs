using EPGManager.Interfaces;
using EPGManager.Models;
using System;
using System.Collections.Generic;

namespace EPGManager.Services
{
    public class EPGService : IEPGService
    {
        private readonly IEPGRepository _repository;
        public EPGService(IEPGRepository repository)
        {
            _repository = repository;
        }

        public void ImporteEPG()
        {
            var channels = new List<Channel>(){
                new Channel
                {
                    Id = "AandE.br",
                    Name = "A&E",
                    Icon = "https://static.epg.best/br/AandE.br.png"
                },
                new Channel
                {
                    Id = "AmazonSat.br",
                    Name = "Amazon Sat",
                    Icon = "https://static.epg.best/br/AmazonSat.br.png"
                },
                new Channel
                {
                    Id = "Amc.br",
                    Name = "AMC",
                    Icon = "https://static.epg.best/br/Amc.br.png"
                },
                new Channel
                {
                    Id = "AnimalPlanet.br",
                    Name = "Animal Planet",
                    Icon = "https://static.epg.best/br/AnimalPlanet.br.png"
                }
            };
            _repository.AddChannels(channels);
             var random = new Random();
            var programmes = new List<Programme>()
            {
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "AandE.br",
                    Title = "CSI",
                    SubTitle = "Split Decisions",
                    Description = "Quando a equipe CSI é chamada para investigar um homem que é baleado à queima-roupa em um cassino local, eles fecham o prédio inteiro para encontrar seu assassino",
                    Category = "Séries, Ação",
                    Icon = "https://cdn.mitvstatic.com/programs/br_csi-s12e19-split-decisions_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 17:05:00"),
                    Stop = DateTime.Parse("2021/03/31 17:55:00"),
                    ChannelId = "AandE.br",
                    Title = "CSI",
                    SubTitle = "Split Decisions",
                    Description = "Quando a equipe CSI é chamada para investigar um homem que é baleado à queima-roupa em um cassino local, eles fecham o prédio inteiro para encontrar seu assassino",
                    Category = "Séries, Ação",
                    Icon = "https://cdn.mitvstatic.com/programs/br_csi-s12e19-split-decisions_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 18:05:00"),
                    Stop = DateTime.Parse("2021/03/31 18:55:00"),
                    ChannelId = "AandE.br",
                    Title = "CSI",
                    SubTitle = "Split Decisions",
                    Description = "Quando a equipe CSI é chamada para investigar um homem que é baleado à queima-roupa em um cassino local, eles fecham o prédio inteiro para encontrar seu assassino",
                    Category = "Séries, Ação",
                    Icon = "https://cdn.mitvstatic.com/programs/br_csi-s12e19-split-decisions_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "AmazonSat.br",
                    Title = "Ervas e Plantas",
                    SubTitle = "",
                    Description = "Um programa pioneiro em mostrar o uso saudável das plantas pelo amazônida e que pretende desvendar os mistérios da união entre o conhecimento popular e a ciência, destacando os benefícios e propriedades das ervas e plantas nativas da Amazônia",
                    Category = "Nature",
                    Icon = "https://cdn.mitvstatic.com/programs/br_ervas-e-plantas_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 17:05:00"),
                    Stop = DateTime.Parse("2021/03/31 17:55:00"),
                    ChannelId = "AmazonSat.br",
                    Title = "Ervas e Plantas",
                    SubTitle = "",
                    Description = "Um programa pioneiro em mostrar o uso saudável das plantas pelo amazônida e que pretende desvendar os mistérios da união entre o conhecimento popular e a ciência, destacando os benefícios e propriedades das ervas e plantas nativas da Amazônia",
                    Category = "Nature",
                    Icon = "https://cdn.mitvstatic.com/programs/br_ervas-e-plantas_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 18:05:00"),
                    Stop = DateTime.Parse("2021/03/31 18:55:00"),
                    ChannelId = "AmazonSat.br",
                    Title = "Ervas e Plantas",
                    SubTitle = "",
                    Description = "Um programa pioneiro em mostrar o uso saudável das plantas pelo amazônida e que pretende desvendar os mistérios da união entre o conhecimento popular e a ciência, destacando os benefícios e propriedades das ervas e plantas nativas da Amazônia",
                    Category = "Nature",
                    Icon = "https://cdn.mitvstatic.com/programs/br_ervas-e-plantas_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "Amc.br",
                    Title = "O Hobbit - A Desolação de Smaug",
                    SubTitle = "",
                    Description = "Ao lado de um grupo de anões e de Gandalf, Bilbo segue em direção à Montanha Solitária, onde deverá ajudar seus companheiros a retomar a Pedra de Arken. O problema é que o artefato está perdido em meio a um tesouro protegido pelo temido dragão Smaug",
                    Category = "Fantasy, Ação",
                    Icon = "https://cdn.mitvstatic.com/programs/br_o-hobbit-a-desolac-o-de-smaug-2013-1_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 17:05:00"),
                    Stop = DateTime.Parse("2021/03/31 17:55:00"),
                    ChannelId = "Amc.br",
                    Title = "S.W.A.T. - Comando Especial 2",
                    SubTitle = "",
                    Description = "Paul Cutler, especialista em táticas antiterroristas e policial figurão do Departamento de Polícia de Los Angeles, é enviado a Detroit a fim de treinar seu time S.W.A.T. nas mais novas técnicas de resgate de reféns. Porém, tudo muda quando um assassino se torna obcecado em exterminar Cutler e todo seu time",
                    Category = "Action, Filme",
                    Icon = "https://cdn.mitvstatic.com/programs/br_s-w-a-t-comando-especial-2-2011-1_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 18:05:00"),
                    Stop = DateTime.Parse("2021/03/31 18:55:00"),
                    ChannelId = "Amc.br",
                    Title = "Amanhecer Violento",
                    SubTitle = "",
                    Description = "O dia estava tranquilo até um grupo de paraquedistas aterrissar no campo de futebol de um colégio. Era o início de uma invasão da Coreia do Norte ao território americano, o que gera pânico nos habitantes da cidade. Decididos a defender o local, oito jovens se escondem nas montanhas e, adotando o nome de sua equipe de futebol, planejam como revidar à invasão das forças militares inimigas",
                     Category = "Action, Filme",
                    Icon = "https://cdn.mitvstatic.com/programs/br_amanhecer-violento-2012-1_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "AnimalPlanet.br",
                    Title = "Os Reis da Selva",
                    SubTitle = "Faces das Sombras",
                    Description = "Depois de meses, o bando retorna à Mara durante a migração dos gnus. O bando Marsh desaparece de vista. Surge uma rara coalizão de cinco guepardos machos. A caçada de Bahati é interrompida pela intervenção de um enorme crocodilo",
                    Category = "Séries",
                    Icon = "https://cdn.mitvstatic.com/programs/br_os-reis-da-selva-s01e04-faces-das-sombras_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "AnimalPlanet.br",
                    Title = "Indochina Selvagem",
                    SubTitle = "Malásia",
                    Description = "A diversidade da Malásia é inigualável, principalmente na terceira maior ilha do mundo, Bornéu, onde orangotangos se balançam entre as árvores enquanto crocodilos gigantes espreitam os rios",
                    Category = "Séries",
                    Icon = "https://cdn.mitvstatic.com/programs/br_indochina-selvagem-s01e04-malasia_l_m.jpg"
                },
                new Programme
                {
                    Id = random.Next(),
                    Start = DateTime.Parse("2021/03/31 16:05:00"),
                    Stop = DateTime.Parse("2021/03/31 16:55:00"),
                    ChannelId = "AnimalPlanet.br",
                    Title = "Nova Zelândia Selvagem",
                    SubTitle = "Mares Violentos",
                    Description = "Com a extinção da vida marinha antiga, uma porta evolucionária se abriu para permitir que tubarões, baleias e outras criaturas prosperassem",
                    Category = "Séries, Ação",
                    Icon = "https://cdn.mitvstatic.com/programs/br_nova-zelandia-selvagem-s01e06-sobreviventes-extremos_l_m.jpg"
                }
            };
            _repository.AddProgrammes(programmes);
        }
    }
}