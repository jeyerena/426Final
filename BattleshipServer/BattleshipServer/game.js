$(document).ready(function () {

    document.getElementById('build').onclick = function () {
		var num = parseInt(document.getElementById('boardSize').value, 10);
        tableCreate(num, "left");
        tableCreate(num, "right");
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        var table = document.getElementById('table-middle');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            cell.onclick = shot;
        }
	};

	function tableCreate(num, position) {
        var body = document.getElementById('board');
        var tbl = document.createElement('table');
		tbl.id = 'table-' + position;
		tbl.style.margin = 20 + 'px';
		tbl.style.tableLayout = 'fixed';
        for (var i = 0; i < num; i++) {
            var tr = tbl.insertRow();
            for (var j = 0; j < num; j++) {
                var td = tr.insertCell();
                td.style.width = (150/num + num-1) + 'px';
                td.style.height = (150/num + num-1) + 'px';
                td.id = i + ',' + j;
               // td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
		body.appendChild(tbl);
		placeShips('a');
	}
	
	function placeShips(ships){
		var p = {}
		p['x'] = 0;
		p['y'] = 0;
		p['length'] = 2;
		p['width'] = 1;
		p['isVertical'] = false;
		var shi = JSON.stringify(p);
		var q = JSON.parse(shi);
		ships = JSON.stringify(ships);		
	}

    function shot() {
		this.classList.toggleClass('w3-red')
        var cell = {};
        cell['x'] = this.id.split(',')[0];
        cell['y'] = this.id.split(',')[1];
        var cell = JSON.stringify(this.id);
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "/json-handler");
        xmlhttp.setRequestHeader("Content-Type", "application/json");
        xmlhttp.send(cell);
  //      this.className = '';
    }
});
