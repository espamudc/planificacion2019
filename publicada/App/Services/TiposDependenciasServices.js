angular.module('appGestion')
    .factory('TiposDependenciasServices', ['$http', function ($http) {
        var fact = {};

        fact.getTipoDependencia = function () {
            return $http.get('../TipoDependencia/TipoDependencia');
        }
        return fact;
    }]);