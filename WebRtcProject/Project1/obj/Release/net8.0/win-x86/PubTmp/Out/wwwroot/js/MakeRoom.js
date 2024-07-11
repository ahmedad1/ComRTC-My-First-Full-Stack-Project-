import { backOrigin, buildSdpAnswer,deleteAllUserCookies, frontOrigin, insertAfter,getCookie, toggleFullScreen, configsOfWebRTC } from "./Shared.js"


let interval
let usernav;
let mystreamobj=null
let videonode=null
let navul=document.querySelector("ul.navbar-nav")
let closeMeetingbtn=document.querySelector(".closeMeeting")
let videobtn=document.querySelector('.videobtn')
let micbtn=document.querySelector('.micbtn')
let screenbtn=document.querySelector('.screenbtn')
let roomName;
let rtcObjectsControl={};
let videoStreams={};
let linkofmeeting =document.querySelector(".linkofmeeting");

let video=document.querySelector(".myvideotag")

let controls;

video.ondblclick=function(){toggleFullScreen(this)}
onload=async function(){
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
    let fullname= getCookie("fullname")
   
    usernav=`<li class="nav-item">
    <span   class="nav-link d-flex "><img src="images/user-solid.svg"style="width: 1vw;margin-right: 0.3vw;" alt=""><span class="fullNameSpan"></span></span>
    </li>`
navul.innerHTML+=usernav;
document.querySelector(".fullNameSpan").innerText+=fullname;

///////////
if(Object.keys(rtcObjectsControl).length==0){
    screenbtn.disabled=true
    document.querySelector(".note").style="display:inline"
   }
   let res=await  fetch(`${backOrigin}/api/Account/MakeNewRoom`,{method:"POST",credentials:"include"})
   res=await res.json();
   roomName=res.roomName;

    linkofmeeting.value=`${frontOrigin}/Join.html?${roomName}`
    let copybtn=document.querySelector(".copylinkbtn")
    copybtn.onclick=async function(){
        await navigator.clipboard.writeText(linkofmeeting.value)
        if(copybtn.value!="Copied"){
        copybtn.value="Copied"
        setTimeout(function(){copybtn.value="Click here to copy"},2000)
        }
    }
    let signal=await new signalR.HubConnectionBuilder().withUrl(`${backOrigin}/connection`,{withCredentials:true}).build();
    await signal.start();
    signal.invoke("AddToGroup",roomName);
    mystreamobj = await navigator.mediaDevices.getUserMedia({
        audio: true,video:true})
    let initTracks=mystreamobj.getTracks()
    for(let i of initTracks){
        i.enabled=false
    }
    video.srcObject=new MediaStream(mystreamobj.getVideoTracks());
    signal.on("getSdpOfUser",async function( username,fullname,sdp1){
        const rtc=new RTCPeerConnection(configsOfWebRTC);
        
        rtc.ontrack=function(e){
            // console.log("ontrackMakeRoom")
            if(videonode==null||videonode.srcObject!=e.streams[0]){
             videonode=document.createElement("video");
             videoStreams[username]=e.streams[0];
            videonode.srcObject=e.streams[0]
            videonode.autoplay=true;
                // mystreamobj=video.srcObject
                mystreamobj=rtc.getLocalStreams()[0]
            let divContainer=document.createElement("div")
            divContainer.classList.add("col-lg-4","col-md-6","col-12","rounded","video-container")
            divContainer.appendChild(videonode);
            // <div class="overlay-text">Video 1 Sharing Ended</div>
            let overlay=document.createElement("div")
            overlay.classList.add("overlay-text")
            overlay.innerText=fullname
            divContainer.appendChild(overlay)
            //col-lg-3 col-md-4 col-sm-6 col-12  rounded myvideotag
            // videonode.classList.add("col-lg-3","col-md-4","col-sm-6","col-12","rounded")
            let videocontainerparent=document.querySelector(".videolist");
            videocontainerparent.appendChild(divContainer)
            videonode.ondblclick=function(){toggleFullScreen(videonode)}
            // insertAfter(videonode,allvideos[allvideos.length-1]);
            }
          
            document.querySelector(".note").style="display:none"
            screenbtn.disabled=false
               
        }
        await rtc.setRemoteDescription({type:"offer",sdp:JSON.parse(sdp1)});
        if(Object.keys(rtcObjectsControl).length==0)
        controls=await buildSdpAnswer(rtc,video);
        else{
            controls=await buildSdpAnswer(rtc,video,rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].getUserStream());

        }
        signal.invoke("ReplySdp",getCookie("username"),username,JSON.stringify(controls.Rtc.localDescription.sdp))
        
    rtcObjectsControl[username]=controls;
 
       
    })
    signal.on("isOffline",function(username){
      

        for(let i of document.querySelectorAll("video")){
            if(i.srcObject==videoStreams[username]){
                let tracks=i.srcObject.getTracks()
                for(let j of tracks){
                    j.stop()
                }
                i.parentElement.remove()
               
                break;
            }
        }
        rtcObjectsControl[username].Rtc.close()
        delete rtcObjectsControl[username]
        if(Object.keys(rtcObjectsControl).length==0){
            screenbtn.disabled=true
           
                toggleScreen()
            
            document.querySelector(".note").style="display:inline"
           }


    })
    

videobtn.onclick=async function(){
    this.classList.toggle("cancelline")
    
    if(Object.keys(rtcObjectsControl).length!=0){
        rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].toggleUserVideo()
        mystreamobj=rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].getUserStream()
    }
        else{
            
            let enabled=videobtn.classList.contains("cancelline")
            if(video.srcObject==null){
            let userstreamm=await navigator.mediaDevices.getUserMedia({video:true,audio:true})
            mystreamobj=userstreamm
            let tracks=userstreamm.getAudioTracks()
            for (let i of tracks){
                i.enabled=false
            }
            video.srcObject=new MediaStream(userstreamm.getVideoTracks())
            }else{
                let tracks=video.srcObject.getVideoTracks()
                mystreamobj=video.srcObject
                for(let i of tracks){
                    i.enabled=enabled;
                }
            }
        }
       
}

micbtn.onclick=async function(){
   
    this.classList.toggle("cancelline")

    if(Object.keys(rtcObjectsControl).length!=0){
    rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].toggleUserAudio()
    mystreamobj=rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].getUserStream()
    }
    else{
        let enabled=micbtn.classList.contains("cancelline")
        if(video.srcObject==null){
            let userstreamm=await navigator.mediaDevices.getUserMedia({video:true,audio:true});
            let tracks=userstreamm.getVideoTracks()
            for (let i of tracks){
                i.enabled=false;
            }
            mystreamobj=userstreamm
            video.srcObject=new MediaStream(userstreamm.getVideoTracks())
            
        }
        else{
            let userstreamm=video.srcObject
            let tracks=userstreamm.getAudioTracks()
            mystreamobj=userstreamm
            for (let i of tracks){
                i.enabled=enabled
            }
        }
    }

}

screenbtn.onclick=toggleScreen;
async function toggleScreen(){
    if(Object.keys(rtcObjectsControl).length==0){
        if(screenbtn.classList.contains("cancelline")){
        screenbtn.classList.toggle("cancelline")
        video.srcObject.getVideoTracks()[0].stop()
        video.srcObject=new MediaStream(mystreamobj.getVideoTracks())    
        }
        screenbtn.disabled=true
        return
       }
   screenbtn.classList.toggle("cancelline")
   
    
    if(document.querySelector(".screenbtn").classList.contains("cancelline")){
        let screenStream;
        try{
            screenStream=await navigator.mediaDevices.getDisplayMedia({video:true,cursor:true})
       }
       catch{
           screenbtn.classList.toggle("cancelline")
          
           return
       }
        let user=video.srcObject
        video.srcObject=screenStream;
        let userstreams={};
        for(let i in rtcObjectsControl){

           let videosender= rtcObjectsControl[i].Rtc.getSenders().find(x=>x.track!=null&&x.track.kind=="video")
             userstreams[i]=rtcObjectsControl[i].getUserStream();
            videosender.replaceTrack(screenStream.getVideoTracks()[0]);
            
        }
        screenStream.getVideoTracks()[0].onended=function(){
        
         for(let i in rtcObjectsControl){
             user=i
             let videosender=rtcObjectsControl[i].Rtc.getSenders().find(x=>x.track!=null&&x.track.kind=="video")
             videosender.track.stop() 
            
             videosender.replaceTrack(mystreamobj.getVideoTracks()[0])

         }
         let tracksofscreen=video.srcObject.getVideoTracks()
         for(let i of tracksofscreen){
            i.stop()
         }
         delete video.srcObject
         video.srcObject=new MediaStream(mystreamobj.getVideoTracks())
      
         screenbtn.classList.toggle("cancelline")

        }

    }else{
     let user;
     let userstream;
     for(let i in rtcObjectsControl){
         user=i;
        userstream=mystreamobj
         
         let videosender=rtcObjectsControl[i].Rtc.getSenders().find(x=>x.track!=null&&x.track.kind=="video")
         videosender.track.stop() //////last Added
        
         videosender.replaceTrack(userstream.getVideoTracks()[0])

     }
        let tracksofscreenn= video.srcObject.getVideoTracks()
        for(let i of tracksofscreenn){
            i.stop()
        }
        delete video.srcObject
        video.srcObject=new MediaStream(mystreamobj.getVideoTracks())
  
    }
 //   controls.toggleSharedScreen(video);
 }

closeMeetingbtn.onclick=function(){
    location.href=`${frontOrigin}/main.html`
}
}


console.log(getCookie("expiration"))
