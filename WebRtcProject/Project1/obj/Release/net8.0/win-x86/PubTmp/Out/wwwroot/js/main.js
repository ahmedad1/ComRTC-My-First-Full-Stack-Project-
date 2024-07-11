import { backOrigin, deleteAllUserCookies, deleteCookie, frontOrigin, getCookie, setCookie } from "./Shared.js"

let usernav;
let navul=document.querySelector("ul.navbar-nav")
let fullname
let signoutbtn=document.querySelector(".signoutbtn")
let interval;
let makeroom=document.querySelector(".makeroom")
let joinroom=document.querySelector('.joinroom')

onload=function(){
    if(getCookie("emailconfirmed")==""||getCookie("expiration")==""){
        deleteAllUserCookies()
    location.href=`${frontOrigin}/index.html`
    return
    }
    if(getCookie("emailconfirmed")==false){
        location.href=`${frontOrigin}/verification.html`
        return
    }
    let cookieExp=new Date(getCookie("expiration"))
    
    if(cookieExp<=new Date()||(cookieExp-new Date())/60000 >60){
        fetch(`${backOrigin}/api/Account/UpdateToken`,{method:"POST",credentials:"include"})
        .then(res=>{
            if(res.status!=200){
                deleteAllUserCookies();
                this.location.href=`${frontOrigin}/index.html`
                
            }
           return res.json()
        }).then(res=>{
            setCookie("expiration",res.expiration,500)
        })
    }
    fullname= getCookie("fullname")
   
    usernav=`<li class="nav-item">
    <span   class="nav-link d-flex "><img src="images/user-solid.svg"style="width: 1vw;margin-right: 0.3vw;" alt=""><span class="fullNameSpan"></span></span>
    </li>`
navul.innerHTML+=usernav;
document.querySelector(".fullNameSpan").innerText+=fullname;



}
makeroom.onclick=function(){
    location.href=`${frontOrigin}/MakeRoom.html`
}
joinroom.onclick=async function(){
try{
    let copied=await navigator.clipboard.readText()
    if(copied.startsWith(frontOrigin))
    location.href=copied
    else{
    alert("You should copy the link of the room first")
    }}
    catch{
        alert("Something Went Wrong , Try Again")
    }
}
signoutbtn.onclick=async function(e){
    e.preventDefault()
    document.querySelector(".signoutloading").style="display:inline";
    deleteAllUserCookies()
    await fetch(`${backOrigin}/api/Account/SignOut`,{method:"POST",credentials:"include"})
    location.href=`${frontOrigin}/index.html`
}
