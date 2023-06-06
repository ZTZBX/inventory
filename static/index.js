window.executinIn = false;
window.itemStak;

function changeInputValue() {

  if (Number($("#quantityDrop").attr('max')) < Number($("#rangevalDrop").val())) {

    $("#rangevalDrop").val(Number($("#quantityDrop").attr('max')))
    $("#quantityDrop").val(Number($("#quantityDrop").attr('max')));

  } else {
    $('#quantityDrop').val(Number($("#rangevalDrop").val()))
    $('#rangevalDrop').val(Number($("#rangevalDrop").val()))
  }
}

function closeDropItemMenu() {
  $(".drop-item-quantity").css('visibility', 'hidden');
  $("#quantityDrop").val('0');
  $("#rangevalDrop").val('0');
  $("#quantityDrop").attr('max', '10');
  $("#dropQuantityName").attr('name', '');
}

function openDropItemMenu(name, quantity) {
  $("#quantityDrop").val('0');
  $("#rangevalDrop").val('0');
  $("#quantityDrop").attr('max', quantity.replace(/\D/g, ''));
  $("#dropQuantityName").attr('name', name);
  $("#dropQuantityName").attr('value', 'Drop ' + name);
  $(".drop-item-quantity").css('visibility', 'visible');
}


function closeGetItemMenu() {
  $(".get-item-quantity").css('visibility', 'hidden');
  $("#quantityGet").val('0');
  $("#rangevalGet").val('0');
  $("#quantityGet").attr('max', '10');
  $("#getQuantityName").attr('name', '');
}

function openGetItemMenu(name, quantity) {
  $("#quantityGet").val('0');
  $("#rangevalGet").val('0');
  $("#quantityGet").attr('max', quantity.replace(/\D/g, ''));
  $("#getQuantityName").attr('name', name);
  $("#getQuantityName").attr('value', 'Get ' + name);
  $(".get-item-quantity").css('visibility', 'visible');
}



function handleDragStart(e) {
  dragSrcEl = this;
  component.set("v.dragid", e.target.dataset.dragId);
  e.dataTransfer.setData('Text', component.id);
  e.dataTransfer.effectAllowed = 'move';
  e.dataTransfer.setData('text/html', this.innerHTML);

}

function handleDrop(e) {
  e.stopPropagation();

  if (dragSrcEl !== this) {
    dragSrcEl.innerHTML = this.innerHTML;
    this.innerHTML = e.dataTransfer.getData('text/html');
  }

  return false;
}

function handleDragEnd(e) {
  items.forEach(function (item) {
    item.classList.remove('over');
  });
}

function handleDragOver(e) {
  if (e.preventDefault) {
    e.preventDefault();
  }

  return false;
}

function handleDragEnter(e) {
  this.classList.add('over');
}

function handleDragLeave(e) {
  this.classList.remove('over');
}


// Opens the individual inventory items
function openItemHandler() {
  let item = $(".item-grid").find(".item");
  let panel;

  item.mouseenter(function (e) {
    if (!$(this).hasClass("empty")) {
      let type = $(this)
        .find("h5")
        .text()
        .replace(/ /g, "")
        .toLowerCase();
      panel = $(".panel." + type);
      panel.css("left", $(this).offset().left + 116).fadeIn(100);

      //width: 112px; to 120
      //height: 112px; to 120

      $(this).css('background-color', 'rgba(27, 27, 27, 0.7)')
      $(this).css('color', 'white')

      $(this).mousedown(
        function (e) {
          panel.fadeOut(100);
        }
      )
    }

  }).mouseleave(function () {
    try {

      panel.fadeOut(100);
      if (!$(this).hasClass("empty")) {
        $(this).css('background-color', 'rgba(27, 27, 27, 0.3)')
        $(this).css('color', 'rgb(27, 27, 27)')
      }

    } catch
    { }

  });
}

function generateItemsInGround(listOfItems) {
  var playerInventory = JSON.parse(listOfItems['data']);
  var list_of_slots = playerInventory

  $("#itemsListGround").empty();


  for (let i = 0; i < list_of_slots.length; i++) {
    slot_name = list_of_slots[i]["name"]
    slot_image_name = list_of_slots[i]["image"]
    cuantity = list_of_slots[i]["quantity"] + " " + list_of_slots[i]["unit"]

    var slot = `
      <div class="item item-trade" cuantity="`+ cuantity + `" id="` + list_of_slots[i]["name"] + `" name="` + list_of_slots[i]["slotposition"] + `" draggable='true'>
      <h5 >`+ slot_name + `</h5>
      <h5 class="itemtradequntitiy">`+ cuantity + `<h5>
      <div class="item-img"><img src="./`+ slot_image_name + `"></div>
      </div>
      `
    $("#itemsListGround").append(slot)
  }


  let item = $(".item-grid-drop-items").find(".item");

  item.mouseenter(function (e) {
    if (!$(this).hasClass("empty")) {
      $(this).css('background-color', 'rgba(27, 27, 27, 0.7)')
      $(this).css('color', 'white')
      $(this).mousedown(
        function (e) {
          openGetItemMenu($(this).attr('id'), $(this).attr('cuantity'));
        }
      )
    }
  }).mouseleave(function () {
    $(this).css('background-color', 'rgba(27, 27, 27, 0.3)')
    $(this).css('color', 'rgb(27, 27, 27)')
  });

}

function generateItemInSlots(listOfItems) {
  //
  //     ITEM SLOT EXAMPLE OF CODE
  // <div class="item item-trade">
  //        <h5>Money</h5>
  //        <div class="item-img"><img src="./money.png"></div>
  //  </div>
  //

  //
  // Panel Example
  // <div class="panel panel-trade money">
  // <div class="bar">
  //          <img src="./money.png">
  //          <h1>Money</h1>
  //          <h4>Information</h4>
  //      </div>
  //      <div class="description">
  //          <p>Money in hand</p>
  //          <p>Although cash typically refers to money in hand.</p>
  //          <div class="quantity">
  //              <img src="./money.png">
  //              <h4>Current Money</h4>
  //              <div class="inv-bar">
  //                  <h1>1250$</h1>
  //              </div>
  //          </div>
  //          <!--
  //          <div class="price">
  //              <h4>Total Value <span>2,750,000 Units</span></h4>
  //              <p>(27,500.0 Units each)</p>
  //              <div class="u">
  //                  <div>U</div>
  //                  <div>U</div>
  //                  <div>U</div>
  //                  <div>U</div>
  //                  <div>U</div>
  //              </div>
  //          </div>
  //         -->
  //          <hr>
  //      </div>
  //      <div class="legend">
  //          <div>
  //              <div>a</div>
  //              <h3>Discard</h3>
  //              <p>Drop in the ground</p>
  //          </div>
  //      </div>
  //</div>


  var playerInventory = JSON.parse(listOfItems["data"]);

  var list_of_slots = playerInventory

  $("#itemsList").empty();
  $(".panel").remove();

  for (let i = 0; i < list_of_slots.length; i++) {
    if (list_of_slots[i]["name"] != "empty") {
      slot_name = list_of_slots[i]["name"]
      slot_image_name = list_of_slots[i]["image"]
      title_description = list_of_slots[i]["descriptiontitle"]
      description = list_of_slots[i]["description"]
      cuantity = list_of_slots[i]["quantity"] + " " + list_of_slots[i]["unit"]

      var slot = `
      <div class="item item-trade" cuantity="`+ cuantity + `" id="` + list_of_slots[i]["name"] + `" name="` + list_of_slots[i]["slotposition"] + `" draggable='true'>
      <h5>`+ slot_name + `</h5>
      <div class="item-img"><img src="./`+ slot_image_name + `"></div>
      `
      var panel = `
      <div class="panel panel-trade `+ slot_name.toLowerCase() + `">
      <div class="bar">
      <img src="./`+ slot_image_name + `">
      <h1>`+ slot_name + `</h1>
      <h4>Information</h4>
      </div>
      <div class="description">
      <p>`+ title_description + `</p>
      <p>`+ description + `</p>
      <div class="quantity">
      <img src="./`+ slot_image_name + `">
      <h4>Current `+ slot_name + `</h4>
      <div class="inv-bar">
      <h1 name="quantity_for_`+slot_name+`">`+ cuantity + `</h1>
      </div>
      </div>
      <hr>
      </div>
      <div class="legend">
      <div>
      <div>D</div>
      <h3>Discard</h3>
      <p>To drop the item, drag to the red area</p>
      </div>
      </div>
      </div>
      `

      $("#itemsList").append(slot)

      $(".mainApp").append(panel)

    } else {
      $("#itemsList").append($(`<div class="item empty" id="empty" name="` + list_of_slots[i]["slotposition"] + `"></div>`))
    }
  }

  let item = $(".item-grid").find(".item");


  let dropItem = $("#itemsDrop")
  let backGround = $(".blur")

  backGround.click(
    function (e) {
      closeDropItemMenu();
      closeGetItemMenu();
    }
  );

  dropItem.droppable({
    accept: '.item',
    drop: function (event, ui) {
      var a = $(this);
      var b = $(ui.draggable);

      var item_name = b.attr("id")
      var item_cuantity = b.attr("cuantity")
      openDropItemMenu(item_name, item_cuantity);

    }
  });

  item.draggable({
    cancel: ".empty",
    helper: "clone",
    revert: true,
    revertDuration: 0,
    scroll: false,
    start: function (e, ui) {
      $(this).css('visibility', 'hidden');
      $(".drop-item").css('visibility', 'visible');
      closeGetItemMenu();
    },
    stop: function () {
      $(this).css('visibility', 'visible');
      $(".drop-item").css('visibility', 'hidden');
    }
  })
    .droppable({
      over: function (event, ui) {
        $(this).effect("highlight", {}, 1000);
      },
      drop: function (event, ui) {
        // Get drag & drop elements
        var a = $(this);
        var b = $(ui.draggable);

        // TODO: DEBUG THIS

        // Changing name between drag and drop
        var helper_b = b.attr("name")
        var helper_a = a.attr("name")
        b.attr("name", helper_a)
        a.attr("name", helper_b)

        // now is time to change this in the database to

        fetch(`https://inventory/change_item_position`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
          },
          body: JSON.stringify({
            itemname: b.attr("id"),
            position: b.attr("name")
          })
        });

        // Swap those elements
        var tmp = $('<span>').hide();
        a.before(tmp);
        b.before(a);
        tmp.replaceWith(b);
      }
    });
  openItemHandler()
}


$(function () {
  var dragElement = null;
  // Animates panel based on mouse movement on page
  function panelMovement() {
    let body = $("body");
    let itemGrid = $(".item-grid");
    let dimensions = {};
    let maxMove = { x: 30, y: 5 };
    let currentPos = { left: 275, top: 50 };

    $(window).resize(function () {
      dimensions.width = body.width();
      dimensions.height = body.height();
    }).resize();

    // Default left positioning is in px, top is in %. Retains value type
    body.mousemove(function (event) {
      let percentageX = event.pageX / dimensions.width;
      let percentageY = event.pageY / dimensions.height;
      itemGrid.css({
        left: currentPos.left - maxMove.x * percentageX,
        top: currentPos.top - maxMove.y * percentageY + "%"
      });
    });
  }

  panelMovement()

  window.addEventListener('message', function (event) {
    var item = event.data;

    if (item.showIn == true) {

      fetch(`https://inventory/get_inventory`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: JSON.stringify({
        })
      }).then(resp => resp.json()).then(success => generateItemInSlots(success));

      fetch(`https://inventory/get_on_ground_items`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: JSON.stringify({
        })
      }).then(resp => resp.json()).then(success => generateItemsInGround(success));

      document.getElementsByClassName("mainApp")[0].style.display = 'block';

    } else {
      document.getElementsByClassName("mainApp")[0].style.display = 'none';
    }
  });



  $("#dropQuantityName").click(function () {
    fetch(`https://inventory/drop_items`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      }, body: JSON.stringify({
        item: $("#dropQuantityName").attr("name"),
        quantity: $("#rangevalDrop").val()
      })
    }).then()
      .catch(err => {
      });

    closeDropItemMenu();
    closeGetItemMenu();
  });

  $("#getQuantityName").click(function () {
    fetch(`https://inventory/get_item`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      }, body: JSON.stringify({
        item: $("#rangevalGet").attr("name"),
        quantity: $("#rangevalDrop").val()
      })
    }).then()
      .catch(err => {
      });

    closeDropItemMenu();
    closeGetItemMenu();
  });

  $("#exit").click(function () {
    closeDropItemMenu();
    closeGetItemMenu();

    fetch(`https://inventory/exit`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
    }).then()
      .catch(err => {
      });
  });
});



