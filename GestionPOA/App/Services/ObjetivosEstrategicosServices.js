﻿angular.module('appGestion')
    .factory('ObjetivosEstrategicosServices', ['$http', function ($http) {
        var fact = {};

        fact.getObjetivosEstrategicos = function () {
            return $http.get('../ObjetivosEstrategicos/GetObjetivosEstrategicos');
        }
        fact.getCumplimintnoOEstrategicos = function () {
            return $http.get('../ObjetivosEstrategicos/GetCumplimientoObjetivosEstrategicos');
        }
        fact.getObjetivosEstrategicosBySubSistema = function (id) {
            return $http.get('../ObjetivosEstrategicos/GetObjetivosEstrategicosPeriodoActual/'+id);
        }
 
        fact.addObjetivoEstrategico = function (ObjetivoEstrategico) {
            var request = $http({
                method: 'POST',
                url: '../ObjetivosEstrategicos/Create',
                data: ObjetivoEstrategico,
                dataType: "json"
            });
            return request;
        }

        fact.updateObjetivoEstrategico = function (ObjetivoEstrategico) {
            
            var request = $http({
                method: 'POST',
                url: '../ObjetivosEstrategicos/Update',
                data: ObjetivoEstrategico,
                dataType: "json"
            });
            return request;
        }

        fact.deleteObjetivosEstrategicos = function (id) {
            var request = $http({
                method: 'POST',
                url: '../ObjetivosEstrategicos/Delete/' + id,
                dataType: "json"
            });
            return request;
        }

        return fact;
    }]);