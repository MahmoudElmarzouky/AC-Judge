function showfilter() {
        
    vis = document.getElementById('form-filter').style.display ;
    if (vis == "block"){
        document.getElementById('form-filter').style.display = "none";

    }
    else {
        document.getElementById('form-filter').style.display ="block";
    }
    }
    function ClearFields() {

        document.getElementById("f.Country").value = "";
        document.getElementById("f.UserScreenName").value = "";
        document.getElementById("f.birthday").value = "0";
        document.getElementById("f.rate").value = "0";
        document.getElementById("f.ratedmatch").value = "0";
        document.getElementById("f.birthday2").value = "9999";
        document.getElementById("f.rate2").value = "9999";
        document.getElementById("f.ratedmatch2").value = "9999";

   }