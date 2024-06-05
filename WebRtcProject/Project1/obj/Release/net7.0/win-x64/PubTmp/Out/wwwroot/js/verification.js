import { backOrigin, frontOrigin, getCookie, setCookie } from "./Shared.js";

let verifyText=document.querySelector(".verifyclass")
let verifybtn=document.querySelector(".verifybtn")
verifyText.onkeydown=function (e) {
    if("VvAaCc".includes(e.key)&&e.ctrlKey ){
        return
    }
    if(e.key.match(/[^0-9]/)&&e.keyCode!=8){
        e.preventDefault();
    }
}

verifyText.onpaste=function(e){
    let cb=e.clipboardData||window.clipboardData;
    if(!cb.getData("text").match(/^\d+$/)){
     
        e.preventDefault()
    }
    
}

onload=async function(){
if(getCookie("email")==""){
return
}
await fetch(`${backOrigin}/api/Account/Verify`,{
method:"POST",
headers:{
    "content-type":"application/json"
},
body:JSON.stringify(getCookie("email"))
})

}
verifybtn.onclick=function(e){
e.preventDefault()
document.querySelector('.verifyloading').style="display:inline !important"
if(verifyText.value=='' ||!/^\d{4,7}$/ig.test( verifyText.value)){
    document.querySelector('.verifyloading').style="display:none !important"
return
}
fetch(`${backOrigin}/api/Account/ConfirmEmail`,{
method:"POST",
credentials:"include",
headers:{
    "content-type":"application/json"
},
body:JSON.stringify({
    
  "email": getCookie("email"),
  "code": +verifyText.value
})
}
).then(res=>{
 return res.json()
}).then(res=>{

    if(res.emailConfirmed==false){
        document.querySelector(".invalidcode").style="display : block !important"
        document.querySelector('.verifyloading').style="display:none !important"
    }
    else{
        setCookie("emailconfirmed",true,500)
        setCookie("username",res.username,500)
        setCookie("fullname",res.fullName,500)
        setCookie("expiration",res.expiration,500)
        
        location.href=`${frontOrigin}/main.html`;
    }
})



}

