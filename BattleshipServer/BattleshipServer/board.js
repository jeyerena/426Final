$(document).ready(function () {
    var minX = 5;
    var maxX = 20;
    var minY = 5;
    var maxY = 20;

    function tableCreate(num, position) {
        var body = document.getElementById('board');
        var tbl = document.createElement('table');
        tbl.className = "w3-display-" + position;
        tbl.id = 'table-' + position;
        tbl.style.width = num * 10 + 'px';
        tbl.style.height = num * 10 + 'px';

        for (var i = 0; i < num; i++) {
            var tr = tbl.insertRow();
            for (var j = 0; j < num; j++) {
                var td = tr.insertCell();
                td.style.width = '10px'
                td.style.height = '10px'
                td.id = i + ',' + j
                td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
        body.appendChild(tbl);
    }
    document.getElementById('build').onclick = function () {
        var num = parseInt(document.getElementById('boardSize').value, 10);
        tableCreate(num, "middle");
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
  //      drag()
        var table = document.getElementById('table-middle');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            console.log(cell.id);
            cell.onclick = selected;
        }
    };

    function selected() {
		this.classList.toggle('w3-red');
        var cell = JSON.stringify(this.id);
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "/json-handler");
        xmlhttp.setRequestHeader("Content-Type", "application/json");
        xmlhttp.send(cell);
  //      this.className = '';
    }

    
});
