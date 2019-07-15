angular.module('appGestion')
    .factory('DepartamentosServices', ['$http', function ($http) {
        var fact = {};

        //fact.getDepartamentos = function () {
        //    return $http.get('../Departamentoes/Departamentos');
        //}

      

        fact.getDepartamentoesbyTipo = function (Identificador) {
            return $http.get('../Departamentoes/GetDepartamentoesbyTipo/' + Identificador);
        }

        return fact;
    }]);