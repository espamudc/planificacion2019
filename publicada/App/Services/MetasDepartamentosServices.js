angular.module('appGestion')
    .factory('MetasDepartamentosServices', ['$http', function ($http) {
        var fact = {};

        fact.getMetasDepartamentos = function () {
            return $http.get('../MetasDepartamentoes/MetasAsignadas');
        }       

        fact.addMetasDepartamentos = function (tipodependencia, metaid, idDependencia) {
            var request = $http({
                method: 'POST',
                url: '../MetasDepartamentoes/Create',
                data: { '_TipoDependenciaID': tipodependencia.id, '_IdentificadorTipoDependencia': tipodependencia.Identificador, '_MetasId': metaid, '_DepartamentoID': idDependencia },
                dataType: "json"
            });
            return request;
        }


        fact.updateMetasDepartamentos = function (id, metaid) {
            var request = $http({
                method: 'POST',
                url: '../MetasDepartamentoes/Update',
                data: { 'id': id, '_MetasId': metaid },
                dataType: "json"
            });
            return request;
        }


        fact.deleteMetasDepartamentos = function (id) {
            var request = $http({
                method: 'POST',
                url: '../MetasDepartamentoes/Delete/' + id,
                dataType: "json"
            });
            return request;
        }

        return fact;
    }]);