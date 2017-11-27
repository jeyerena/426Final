$(document).ready(function () {
    document.getElementById('build').onclick = function () {
        var num = parseInt(document.getElementById('Enter rows here:').value, 10);
        function tableCreate(num) {
            var body = document.body,
                tbl = document.createElement('table');
            tbl.style.width = '100px';

            for (var i = 0; i < num; i++) {
                var tr = tbl.insertRow();
                for (var j = 0; j < num; j++) {
                    var td = tr.insertCell();
                    td.appendChild(document.createTextNode('Cell'));
                    td.style.border = '1px solid black'
                }
            }
            body.appendChild(tbl);
        }
        tableCreate(num);

    };
});