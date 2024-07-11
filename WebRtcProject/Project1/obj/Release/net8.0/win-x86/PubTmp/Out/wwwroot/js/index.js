
let signupBtn=document.querySelector('button.signupbtn')
let sendbtn=document.querySelector(".sendbtn")
let fullName=document.querySelector('#fullname')
let email=document.querySelector('#email')
let usernameSignup=document.querySelector('#username_signup')
let passwordSignup=document.querySelector('#password_signup')
///////
let loginBtn=document.querySelector('button.loginbtnform')
let usernameLogin=document.querySelector('#username_login')
let passwordLogin=document.querySelector('#password_login')
///
let showPasswdLogin=document.querySelector("#showpasswdlogin");
let showPasswdSignup=document.querySelector("#showpasswdsignup");
import { backOrigin, deleteAllUserCookies, getCookie } from "./Shared.js"
import { frontOrigin } from "./Shared.js"
import { setCookie } from "./Shared.js"
onload=()=>{
    if(getCookie("emailconfirmed")==false||getCookie("emailconfirmed")==""){
return
    }
if(getCookie("email")==""||getCookie("username")==""){
return 
}
else{
  
    fetch(`${backOrigin}/api/Account/Load`,{
        method:"POST",
        headers:{"content-type":"application/json"},
        body:JSON.stringify({
            "email": getCookie("email"),
            "username": getCookie("username")
          }),
          credentials:"include"
    }).then(res=>res.json())
      .then(res=>{
        if(res.matched){
            location.href=`${frontOrigin}/main.html`;

        }
        else {

            deleteAllUserCookies();
            
        } 
    })
}
}

showPasswdLogin.onchange=function(){
    if(this.checked)
    passwordLogin.setAttribute("type","text")
    else{
        passwordLogin.setAttribute("type","password")
    }
}
showPasswdSignup.onchange=function(){
    if(this.checked)
    passwordSignup.setAttribute("type","text")
    else{
    passwordSignup.setAttribute("type","password")
    }
}

import {resRecaptcha as tokenCaptacha} from "./cap.js"
signupBtn.onclick=function(e){

    e.preventDefault();
    const gcaptcha=tokenCaptacha
    if(gcaptcha.length==0){
        alert("You should fill the form and solve the recaptcha")
        return
    }

    document.querySelector(".signuploading").style="display:inline !important"
if(usernameSignup.value==''){
    document.querySelector('.invalidusername').style="display:block !important;"
    document.querySelector(".signuploading").style="display:none !important"
    return;
}
if(!(/^[a-z_.]+([0-9]*[a-z]*)*@[a-z\d]+/ig.test(email.value))){
    document.querySelector('.invalidemail').style="display:block !important;"
    document.querySelector(".signuploading").style="display:none !important"
    return;
}
if(fullName.value==''){
    document.querySelector('.invalidfullname').style="display:block !important;"
    document.querySelector(".signuploading").style="display:none !important"
    return;
}
if(!(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/gm.test(passwordSignup.value))){
    document.querySelector('.weakpassword').style="display:block !important;"
    document.querySelector(".signuploading").style="display:none !important"
    return;
}
fetch(`${backOrigin}/api/Account/SignUp`,{

    method:"POST",
    headers:{
        "content-type":"application/json"
    },
    body:JSON.stringify({
        "fullName": fullName.value,
        "userName": usernameSignup.value,
        "email": email.value,
        "password": passwordSignup.value,
        "recaptcha":gcaptcha
    })
}).then(res=>{
    if(res.status==200){
   
        setCookie("fullname",fullName.value,500)
        setCookie("email",email.value,500)
        setCookie("username",usernameSignup.value,500)
        setCookie("emailconfirmed",false,500)
        location.href=`${frontOrigin}/verification.html`
    }
    else if(res.status==403){
        grecaptcha.reset()
        console.log("you should solve the recaptcha")
        alert("you should solve the recaptcha")
        return "null"
    }
    else{
        document.querySelector(".signuploading").style="display:none !important"
    }
})



}

import {resRecaptcha as tokenCaptachaLogin} from "./cap.js"
loginBtn.onclick=async function(e){
    e.preventDefault()
    const gcaptcha=tokenCaptachaLogin
    if(gcaptcha.length==0){
        alert("You should fill the form and solve the recaptcha")
        return
    }
   
    document.querySelector(".loginloading").style="display:inline !important"
    if(usernameLogin.value==''){
        document.querySelector(".invalidusernamelogin").style="display: block !important";
        document.querySelector(".loginloading").style="display:none !important";
        return
    }
    if(passwordLogin.value==''){
        document.querySelector('.invalidpasswordlogin').style="display:block !important";
        document.querySelector(".loginloading").style="display:none !important"
        return
    }
     fetch(`${backOrigin}/api/Account/Login`,{
        method:"POST",
        headers:{
            "content-type":"application/json"
        },
        credentials:"include"
        ,
        body:JSON.stringify({
            "userName": usernameLogin.value,
            "password": passwordLogin.value,
            "recaptcha":gcaptcha
          })

    }) .then(res=>{

        if(res.status==200){
            return res.json()
        }
        else if(res.status==403){
            console.log("you should solve the recaptcha")
            alert("you should solve the recaptcha")
            grecaptcha.reset()
            return "null"
        }
        else{
            document.querySelector(".wronguorp").style="display:block !important;"
            grecaptcha.reset()
            return "null";
        }
    }).then(res=>{
        if(res=="null"){
            // grecaptcha.reset()
            document.querySelector(".loginloading").style="display:none !important"
        return
        }

    if(res.emailConfirmed==false){
        setCookie("email",res.email)
        location.href=`${frontOrigin}/verification.html`
        return
    }
   
        setCookie("fullname",res.fullName,500)
        setCookie("email",res.email,500)
        setCookie("username",res.username,500)
        setCookie("emailconfirmed",res.emailConfirmed,500)
        setCookie("expiration",res.expiration,500)
        
         location.href=`${frontOrigin}/main.html`
    })
    

}


