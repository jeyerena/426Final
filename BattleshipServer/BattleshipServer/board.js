$(document).ready(function () {
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
        tableCreate(num, "topmiddle");
        tableCreate(num, "middle");
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        drag()
    };

    function drag() {
        var isMouseDown = false,
            isHighlighted;
        var startCell, endCell;
        $("#table-middle td")
            .mousedown(function () {
                isMouseDown = true;
                startCell = this;
                $(this).toggleClass("w3-red");
                isHighlighted = $(this).hasClass("w3-red");
                return false;
            })
            .mouseover(function () {
                if (isMouseDown) {
                    endCell = this;
                    var startX = startCell.id.split(',')[0]
                    var startY = startCell.id.split(',')[1]
                    var endX = endCell.id.split(',')[0]
                    var endY = endCell.id.split(',')[1]
                    if (endX < startX) {
                        var tmp = startX;
                        startX = endX;
                        endX = tmp;
                    }
                    if (endY < startY) {
                        var tmp = startY;
                        startY = endY;
                        endY = tmp;
                    }
                    for (i = startX; i <= endX; i++) {
                        for (j = startY; j <= endY; j++) {
                            var cellID = i + ',' + j;
                            var highlightedCell = document.getElementById(cellID);
                            $(highlightedCell).toggleClass("w3-red", isHighlighted);
                        }
                    }
                }
            });

        $(document)
            .mouseup(function () {
                isMouseDown = false;
            });
    }

    var table = document.getElementById('table');
});
