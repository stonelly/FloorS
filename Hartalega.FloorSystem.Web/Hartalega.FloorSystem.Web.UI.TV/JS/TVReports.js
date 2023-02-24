function MaxMinGrid(Grid,GridCell, Footer, ActualScreenHeight, MainDiv, Body) {
    var elem = GridCell;
    var GridFooterHeight = Footer.innerHeight();
    var ScreenHeight = ActualScreenHeight - 25;
    var mainDivWidth = MainDiv.innerWidth();
    var elemfontSize = parseInt(elem.css('font-size'));
    
    while ((elem.innerWidth() < mainDivWidth) && (Body.height() < ScreenHeight)) {
        elemfontSize++;
        elem.css('font-size', elemfontSize + 'px');
    }
    while ((mainDivWidth < elem.innerWidth() || ScreenHeight < Body.height()) && elemfontSize > 8) {
        elemfontSize--;
        elem.css('font-size', elemfontSize + 'px');
    }
    var elemHeight = parseInt(Grid.css('height'));
    while (Body.height() < ScreenHeight && !isNaN(elemHeight)) {
        elemHeight++;
        Grid.css('height', elemHeight + 'px');
    }
}