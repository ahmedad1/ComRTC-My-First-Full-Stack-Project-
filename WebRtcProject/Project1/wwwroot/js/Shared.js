export let backOrigin =location.origin   
export let frontOrigin=location.origin
let openedvideo=0;
let openedscreen=0;
let openedmic=0;

export function setCookie(cookieName, cookieValue, expirationDays) {
    const d = new Date();
    d.setTime(d.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + d.toUTCString();
    const encodedValue = encodeURIComponent(cookieValue); // Encoding the cookie value
    document.cookie = cookieName + "=" + encodedValue + ";" + expires + ";path=/";
  }
  
export function getCookie(cookieName) {
    const name = cookieName + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(';');
    for(let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i];
      while (cookie.charAt(0) === ' ') {
        cookie = cookie.substring(1);
      }
      if (cookie.indexOf(name) === 0) {
        return cookie.substring(name.length, cookie.length);
      }
    }
    return "";
  }
  export function deleteCookie(cookieName) {
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
  }
  console.clear()
  console.log("%cWARNING: Don't Copy and Paste any code here that anyone gives you because this code may make him access your account","font-size:25px;color:red;font-weight:bold;text-shadow: .7px .7px black;")
  export function deleteAllUserCookies(){
    deleteCookie("email")
    deleteCookie("emailconfirmed")
    deleteCookie("fullname")
    deleteCookie("username")
    deleteCookie("expiration")
  }
  export function toggleFullScreen(video){
    
    if(video.requestFullscreen)
    video.requestFullscreen()
    else if (video.webkitRequestFullScreen){
        video.webkitRequestFullScreen()
    }
    else if (video.mozRequestFullScreen){
        video.mozRequestFullScreen()
    }
    else if(video.msRequestFullScreen){
        video.msRequestFullScreen()
    }

}



  export async function buildSdpOffer(videotag,userstream=null){ 
    let rtc=new RTCPeerConnection();
      let isnull=false
      if(userstream==null){
        isnull=true
       userstream=await navigator.mediaDevices.getUserMedia({video:true,audio:true});
      }
      let tracks=userstream.getTracks();
      for(let i of tracks){
        if(isnull)
        i.enabled=false
        rtc.addTrack(i,userstream);
      }
      videotag.srcObject=new MediaStream(userstream.getVideoTracks())
   
      
  
    let offer=await rtc.createOffer();
    await rtc.setLocalDescription(offer);
    return new Promise((res,rej)=>{
  
      rtc.onicecandidate= function(){
        if(rtc.iceGatheringState=="complete"){
          res({
            Rtc:rtc,
            enableUserVideo: function (){
              let videoTracks=userstream.getVideoTracks();
              for(let i of videoTracks){
                i.enabled=true;
              }
              
          
            },
            disableUserVideo:function(){
              let videoTracks=userstream.getVideoTracks();
              for(let i of videoTracks){
                i.enabled=false;
              }
             
  
        
            },
            toggleUserVideo:function(){
            //  if(!openedvideo){
            //   openedvideo=1;
            //   this.enableUserVideo();


            //  }
            //  else{
            //   openedvideo=0;
            //   this.disableUserVideo();
            //  }
            let videotracks=userstream.getVideoTracks()
            for (let i of videotracks){
              i.enabled=!i.enabled;
            }



            },
            enableUserAudio:function(){
              let audioTracks=userstream.getAudioTracks();
              for(let i of audioTracks){
                i.enabled=true;
              }
              
            },
            disableUserAudio:function(){
              let audioTracks=userstream.getAudioTracks();
              for(let i of audioTracks){
                i.enabled=false;
              }
  
            },
            
            getUserStream:function(){
              return userstream
            },
            toggleUserAudio:function(){
              // if(!openedmic){
              //   openedmic=1;
              //   this.enableUserAudio();
              // }else{
              //   openedmic=0;
              //   this.disableUserAudio()
              // }
              let audioTracks=userstream.getAudioTracks()
              for(let i of audioTracks){
                i.enabled=!i.enabled
              }


            },
            enableSharedScreen:async function (videotag){
              let screenstream=await navigator.mediaDevices.getDisplayMedia({video:true,cursor:true})
              let sendervideo= rtc.getSenders().find(x=>x.track!=null && x.track.kind=="video")
              let tracksofscreen=screenstream.getVideoTracks()[0];
            
              sendervideo.replaceTrack(tracksofscreen);
              
              tracksofscreen.onended=function(){
                sendervideo.replaceTrack(userstream.getVideoTracks()[0])
                videotag.srcObject=new MediaStream(userstream.getVideoTracks())
                document.querySelector(".screenbtn").classList.toggle("cancelline")
              }
              // videotag.setAttribute("controls",true)
              videotag.srcObject=screenstream
             
            },
            disableSharedScreen:function(videotag){
              //  let screenTrack=userstream.getTracks().find(x=>x.kind=="video");
               let screenSender=rtc.getSenders().find(x=>x.track!=null&&x.track.kind=="video")
               screenSender.track.stop()
               screenSender.replaceTrack(userstream.getVideoTracks()[0])
             
                videotag.srcObject=new MediaStream(userstream.getVideoTracks())
                
              
              // videotag.setAttribute("controls",false)
            },
            toggleSharedScreen:function(videotag){
              // if(!openedscreen){
              //   openedscreen=1;
              //   this.enableSharedScreen(videotag);

              // }
              // else{
              //   openedscreen=0;
              //   this.disableSharedScreen(videotag);
              // }
              if( document.querySelector(".screenbtn").classList.contains("cancelline")){
                this.disableSharedScreen(videotag)
              }
              else{
                this.enableSharedScreen(videotag)
              }
            }

          
  
          
          });
        }
      }
    });
    }
    
    export async function buildSdpAnswer(rtc,videotag,userstream=null){
          
      let isnull=false
      // let screenstreamg=null;
      // if(videotag.srcObject!=null&&document.querySelector(".screenbtn").classList.contains("cancelline")){
      //   screenstreamg=videotag.srcObject
      //  }
       if(userstream==null){ 
        isnull=true;
       
        userstream=await navigator.mediaDevices.getUserMedia({video:true, audio:true});
       }
      //  else if(videotag.srcObject!=null){
      //   userstream=videotag.srcObject
      //  }
      // videotag.srcObject=new MediaStream(userstream.getVideoTracks())
      let tracks=userstream.getTracks();
      for(let i of tracks){
        if(isnull)
        i.enabled=false
        rtc.addTrack(i,userstream);
      }
      // if(screenstreamg!=null){
      //   let t=screenstreamg.getVideoTracks()
      //   for(let i of t){
      //     rtc.addTrack(i,screenstreamg);
      //   }
      //   t[0].onended=async function(){
      //     let videosender=rtc.getSenders().find(x=>x.track!=null&&x.track.kind=="video")
      //     videosender.track.stop()
      //     await videosender.replaceTrack(userstream.getVideoTracks()[0])
      //     videotag.srcObject=userstream
      //     document.querySelector(".screenbtn").classList.remove("cancelline")
      //   }
      // }
    // if(screenstreamg==null)
    videotag.srcObject=new MediaStream(userstream.getVideoTracks())
    //  else{
    //   videotag.srcObject=screenstreamg
    //  }
  
      let answer=await rtc.createAnswer();
      await rtc.setLocalDescription(answer);
      return new Promise((res,rej)=>{
  
        rtc.onicecandidate= function(){
          if(rtc.iceGatheringState=="complete"){
            res({
              Rtc:rtc,
              enableUserVideo:function(){
                let videoTracks=userstream.getVideoTracks();
  
                for(let i of videoTracks){
                  i.enabled=true;
                }
               
              },
              disableUserVideo:function(){
                let videoTracks=userstream.getVideoTracks();
                for(let i of videoTracks){
                  i.enabled=false;
                }
               
              },
              toggleUserVideo:function(){
              //  if(!openedvideo){
              //   openedvideo=1;
              //   this.enableUserVideo();


              //  }
              //  else{
              //   openedvideo=0;
              //   this.disableUserVideo();
              //  }
              let videotracks=userstream.getVideoTracks();
              for(let i of videotracks){
                i.enabled=!i.enabled;

              }
              }
              ,

              enableUserAudio:function(){
                let audioTracks=userstream.getAudioTracks();
                for(let i of audioTracks){
                  i.enabled=true;
                }
              },
              disableUserAudio:function(){
                let audioTracks=userstream.getAudioTracks();
                for(let i of audioTracks){
                  i.enabled=false;
                }
              },
              
              toggleUserAudio:function(){
                // if(!openedmic){
                //   openedmic=1;
                //   this.enableUserAudio();
                // }else{
                //   openedmic=0;
                //   this.disableUserAudio()
                // }
                let audioTracks=userstream.getAudioTracks()
                for(let i of audioTracks){
                  i.enabled=!i.enabled
                }


              }
              ,
              enableSharedScreen:async function(videotag){
                
                let screenstream=await navigator.mediaDevices.getDisplayMedia({video:true,cursor:true})
                let sendervideo= rtc.getSenders().find(x=>x.track!=null && x.track.kind=="video")
                
                let tracksofscreen=screenstream.getVideoTracks()[0];
              
                sendervideo.replaceTrack(tracksofscreen);
                
                tracksofscreen.onended=function(){
                  sendervideo.replaceTrack(userstream.getVideoTracks()[0])
                  videotag.srcObject=new MediaStream(userstream.getVideoTracks())
                  document.querySelector(".screenbtn").classList.toggle("cancelline")
                }
                // videotag.setAttribute("controls",true)
                videotag.srcObject=screenstream
              },
              getSharedScreenStream:async function(){
                let screenstream=await navigator.mediaDevices.getDisplayMedia({video:true,cursor:true})
                
                return  screenstream

              },
              getUserStream:function(){
                return userstream
              }
              ,
              disableSharedScreen:function(videotag){
                let sendervideo=rtc.getSenders().find(x=>x.track!=null && x.track.kind=="video");
                sendervideo.track.stop()
                sendervideo.replaceTrack(userstream.getVideoTracks()[0])
                
                // videotag.setAttribute("controls",false)
               
                videotag.srcObject=new MediaStream(userstream.getVideoTracks())
  
              
              },
              toggleSharedScreen:async function(videotag){
                if( !document.querySelector(".screenbtn").classList.contains("cancelline")){
                 
                  await this.enableSharedScreen(videotag);

                }
                else{
                 
                 await  this.disableSharedScreen(videotag);
                }
              }
            
            
            });
          }
        }
      });
    }
  
  
export function insertAfter(sourceNode, destNode){
  destNode.parentNode.insertBefore(sourceNode,destNode.nextSibling);

}

export async function buildSignalR(){
  let conn=new signalR.HubConnectionBuilder().withUrl(`${backOrigin}/connection`,{withCredentials:true}).build()
  await conn.start();
  return conn;
}


import {resRecaptcha} from "./cap.js"
let sendbtn=document.querySelector(".sendbtn")
sendbtn.onclick=async function(e){
    e.preventDefault();
    if(document.querySelector("#email-contact").value.length>0&&!/[a-zA-Z0-9]+@[A-Za-z0-9]+.[a-zA-Z0-9]+.(.[0-9A-Za-z])?/ig.test(document.querySelector("#email-contact").value)){
      alert("Email should be filled in the correct formal way")
      return
    }
    if(document.querySelector("#messagebody").value.length==0){
      alert("Message Body Should Be Filled")
      return
    }
   
    const gcaptcha=resRecaptcha
   
    if(gcaptcha.length==0){
        alert("You should fill the required inputs in the form and solve the recaptcha")
        return;
    }
    document.querySelector(".contactusloading").style="display:inline !important"
   
    fetch(`${backOrigin}/api/Account/ContactUs`,{
        method:"POST",
        headers:{
            "content-type":"application/json"
        },
        body:JSON.stringify({
            "email": document.getElementById("email-contact").value,
            "message": document.getElementById("messagebody").value,
            "recaptcha": gcaptcha})
    })
    .then(res=>{
        if(res.status!=200){
            document.querySelector(".contactusloading").style="display:none !important"
            grecaptcha.reset()
          
            alert("You Didnt Fill All The Mandatory things (reCaptcha Or the message body) ")
        }
        else{
            document.querySelector(".contactusloading").style="display:none !important"
    
            alert("Message Sent Successfully")
        }
    })
    }  