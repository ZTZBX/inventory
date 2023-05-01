//Just a bit of good ol' jquery
(function($) {
    $(document).ready(function() {
      
      // Opens the individual inventory items
      function openItemHandler() {
        let item = $(".item-grid").find(".item");
        let panel;
        item.mouseenter(function() {
            if (!$(this).hasClass("empty")) {
              let type = $(this)
                .find("h5")
                .text()
                .replace(/ /g, "")
                .toLowerCase();
              panel = $(".panel." + type);
              panel.css("left", $(this).offset().left + 116).fadeIn(100);
            }
          }).mouseleave(function() {
            panel.fadeOut(100);
          });
      }
  
      // Animates panel based on mouse movement on page
      function panelMovement() {
        let body = $("body");
        let itemGrid = $(".item-grid");
        let dimensions = {};
        let maxMove = { x: 30, y: 5 };
        let currentPos = { left: 100, top: 50 };
  
        $(window).resize(function() {
            dimensions.width = body.width();
            dimensions.height = body.height();
          }).resize();
  
        // Default left positioning is in px, top is in %. Retains value type
        body.mousemove(function(event) {
          let percentageX = event.pageX / dimensions.width;
          let percentageY = event.pageY / dimensions.height;
          itemGrid.css({
            left: currentPos.left - maxMove.x * percentageX,
            top: currentPos.top - maxMove.y * percentageY + "%"
          });
        });
      }
  
      openItemHandler();
      panelMovement();
    });
  })(jQuery);
//Just a bit of good ol' jquery
(function($) {
  $(document).ready(function() {
    
    // Opens the individual inventory items
    function openItemHandler() {
      let item = $(".item-grid").find(".item");
      let panel;
      item.mouseenter(function() {
          if (!$(this).hasClass("empty")) {
            let type = $(this)
              .find("h5")
              .text()
              .replace(/ /g, "")
              .toLowerCase();
            panel = $(".panel." + type);
            panel.css("left", $(this).offset().left + 116).fadeIn(100);
          }
        }).mouseleave(function() {
          panel.fadeOut(100);
        });
    }

    // Animates panel based on mouse movement on page
    function panelMovement() {
      let body = $("body");
      let itemGrid = $(".item-grid");
      let dimensions = {};
      let maxMove = { x: 30, y: 5 };
      let currentPos = { left: 100, top: 50 };

      $(window).resize(function() {
          dimensions.width = body.width();
          dimensions.height = body.height();
        }).resize();

      // Default left positioning is in px, top is in %. Retains value type
      body.mousemove(function(event) {
        let percentageX = event.pageX / dimensions.width;
        let percentageY = event.pageY / dimensions.height;
        itemGrid.css({
          left: currentPos.left - maxMove.x * percentageX,
          top: currentPos.top - maxMove.y * percentageY + "%"
        });
      });
    }

    openItemHandler();
    panelMovement();
  });
})(jQuery);
  

$(function () {
    
    window.addEventListener('message', function (event) {
        var item = event.data;

        if (item.showIn == true) {
            document.getElementsByClassName("main")[0].style.display = 'block';
        }
    });
});