$(document).ready(function () {
	var num;

    document.getElementById('build').onclick = function () {
		num = parseInt(document.getElementById('boardSize').value, 10);
		tableCreate(num, "top");
		tableCreate(num, "bottom");
		
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        var table = document.getElementById('table-top');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            cell.onclick = shot;
		}
		//have it receive the normal player data from the server
		//or pullit from previous page
		var tmp = []
		tmp.push({'x':0, 'y':0, 'length':2, 'isVertical':true})
		tmp.push({'x':2, 'y':0, 'length':4, 'isVertical':false})			
		tmp.push({'x':2, 'y':4, 'length':2, 'isVertical':false})	
		tmp.push({'x':4, 'y':6, 'length':1, 'isVertical':false})	
		tmp.push({'x':7, 'y':5, 'length':3, 'isVertical':true})	
		tmp.push({'x':9, 'y':9, 'length':1, 'isVertical':true})			
		placeShips('bottom', JSON.stringify(tmp));
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
                td.id = position + '-' + i + ',' + j;
               // td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
		body.appendChild(tbl);
	}
	
	function placeShips(position, shipJSON){
		ships = JSON.parse(shipJSON);
		//check if it's array
		for (var i = 0; i < ships.length; i++){
			var ship = ships[i];
			if (ship.isVertical){
				if (ship.x >= 0 && ship.x < num && ship.y >= 0){
					for (var j = 0; j < ship.length; j++){
						if (ship.y + j < num){
							var cellID = position + '-' + (ship.y + j) + ',' + ship.x;									
							console.log(cellID);
							document.getElementById(cellID).classList.toggle('w3-blue');
						}
					}
				}
			}
			else{
				if (ship.y >= 0 && ship.y < num && ship.x >= 0){
					for (var j = 0; j < ship.length; j++){
						if (ship.x + j < num){
							var cellID = position + '-' + ship.y + ',' + (ship.x + j);	
							console.log(cellID);
							document.getElementById(cellID).classList.toggle('w3-blue');
						}
					}
				}
			}
		}
	}

    function shot() {
		this.classList.toggle('w3-red');
        var cell = {};
        cell['x'] = this.id.split(',')[0];
        cell['y'] = this.id.split(',')[1];
        var cell = JSON.stringify(this.id);
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "/json-handler");
        xmlhttp.setRequestHeader("Content-Type", "application/json");
		xmlhttp.send(cell);
		//display message based on server response
		var message = document.createElement('p');
		message.innerHTML = 'Hit!';
		document.getElementById('events').appendChild(message);
  //      this.className = '';
    }
});
