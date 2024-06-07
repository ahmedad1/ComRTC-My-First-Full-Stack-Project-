import { backOrigin, buildSdpOffer,buildSdpAnswer, buildSignalR, deleteAllUserCookies, frontOrigin, getCookie ,insertAfter, toggleFullScreen} from "./Shared.js"
let usernav;
let interval;
let mystreamobj;
let videonode=null
let navul=document.querySelector("ul.navbar-nav")
let cancels=document.querySelectorAll(".cancel")
let rtcObjectsControl={}
let videoStreams={}
let usersOfGroup=[];
let closeMeetingbtn=document.querySelector(".closeMeeting")
let videobtn=document.querySelector('.videobtn')
let micbtn=document.querySelector('.micbtn')
let screenbtn=document.querySelector('.screenbtn')
let controls={}
let video=document.querySelector(".myvideotag")
video.ondblclick=function(){toggleFullScreen(this)}
onload= async function(){
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
    
   }
  
if(this.location.search.slice(1).length!=0){
let res=await fetch(`${backOrigin}/api/Account/CheckRoom`,{
method:"POST",
headers:{
    "content-type":"application/json"
}
,credentials:"include",
body:JSON.stringify(location.search.slice(1))



})
 
if(res.status!=200){
    alert("Room link is not related to a specific room")
    
    location.href=`${frontOrigin}/main.html`;
}


   





}    
else{
    location.href=`${frontOrigin}/main.html`;
}

let signal=await buildSignalR();


signal.invoke("AddToGroup",this.location.search.slice(1))
signal.on("getUsers",async function(users){

usersOfGroup=users;

let myusername=getCookie("username")
for (let i of usersOfGroup){
    if(i.userName!=myusername){
        if(Object.keys(rtcObjectsControl).length==0)
        controls=await buildSdpOffer(video)
        else{
            controls=await buildSdpOffer(video,rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].getUserStream())
            
        }
        controls.Rtc.ontrack=function(e){
            if(videonode==null||videonode.srcObject!=e.streams[0]){
            videoStreams[i.userName]=e.streams[0];
            videonode=document.createElement("video");
            videonode.autoplay=true;
            // videonode.classList.add("col-lg-3","col-md-4","col-sm-6","col-12","rounded")
            videonode.srcObject=e.streams[0]
            // let allvideos=document.querySelectorAll("video");
                mystreamobj=video.srcObject
            let divContainer=document.createElement("div")
            divContainer.classList.add("col-lg-4","col-md-6","col-12","rounded","video-container")
            divContainer.appendChild(videonode);
            // <div class="overlay-text">Video 1 Sharing Ended</div>
            let overlay=document.createElement("div")
            overlay.classList.add("overlay-text")
            overlay.innerText=i.fullName
            divContainer.appendChild(overlay)

            let videocontainerparent=document.querySelector(".videolist");
            videocontainerparent.appendChild(divContainer)
            videonode.ondblclick=function(){toggleFullScreen(this)}
            // insertAfter(videonode,allvideos[allvideos.length-1]);
            document.querySelector(".note").style="display:none"
            screenbtn.disabled=false
            }
        }
       
    
        rtcObjectsControl[i.userName]=controls;
    signal.invoke("SendSdp",myusername,getCookie("fullname"),JSON.stringify(controls.Rtc.localDescription.sdp),i.userName)

    }

}

})
signal.invoke("GetUsersOfGroup",this.location.search.slice(1))
signal.on("getSdpReplyOfUser",async function(username,sdp1){

   rtcObjectsControl[username].Rtc.setRemoteDescription({type:"answer",sdp:JSON.parse(sdp1)});

})

    signal.on("getSdpOfUser",async function( username,fullname,sdp1){
        let rtc=new RTCPeerConnection();
        
        rtc.ontrack=function(e){
            // console.log("ontrackMakeRoom")
            if(videonode==null||videonode.srcObject!=e.streams[0]){
                videonode=document.createElement("video");
                videoStreams[username]=e.streams[0];
               videonode.srcObject=e.streams[0]
               videonode.autoplay=true;
   
               let divContainer=document.createElement("div")
               divContainer.classList.add("col-lg-4","col-md-4","col-md-6","col-12","rounded","video-container")
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
            if(Object.keys(rtcObjectsControl).length>0){
                screenbtn.disabled=false
               }
        }
        await rtc.setRemoteDescription({type:"offer",sdp:JSON.parse(sdp1)});
        if(Object.keys(rtcObjectsControl).length==0)
        controls=await buildSdpAnswer(rtc,video,null , micbtn.classList.contains("cancelline"));
        else{
            controls=await buildSdpAnswer(rtc,video,rtcObjectsControl[Object.keys(rtcObjectsControl)[0]].getUserStream(),micbtn.classList.contains("cancelline"));

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
        
    screenbtn.disabled=true;

    return;
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
            
             videosender.replaceTrack(userstreams[i].getVideoTracks()[0])

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
     let userstream=mystreamobj;
     for(let i in rtcObjectsControl){
         user=i;
         
         
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



