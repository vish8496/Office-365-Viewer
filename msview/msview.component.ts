import { HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MsserviceService } from './msservice.service';

@Component({
  selector: 'app-msview',
  templateUrl: './msview.component.html',
  styleUrls: ['./msview.component.scss']
})
export class MsviewComponent implements OnInit {
  @ViewChild('officeFrameholder') officeFrameholder: ElementRef;
  @ViewChild('officeForm') officeForm: ElementRef;

  accessToken: any;
  accessTokenTTL:any;
  actionUrl: any;
  docid:any;

  constructor(private msservice:MsserviceService, private route:Router) {
    this.docid = localStorage.getItem("guid");
    if(this.docid == null || this.docid == "")
    {
      this.route.navigate(["allfolders"]);
    }
    else
    {
      this.getActionUrlForDocument();
    }

  }
  ngOnInit(): void {

  }


  ViewDoc()
{
 
  const officeFrame = document.createElement('iframe');
      officeFrame.name = 'officeFrame';
      officeFrame.id = 'officeFrame';
      officeFrame.setAttribute("style" , "inset: 0px;display: block;position: absolute;background-color: transparent;border: none; width: 100%; height: 100%;");
      officeFrame.title = 'Office Online Frame';
      officeFrame.setAttribute('allowfullscreen', 'true');
      officeFrame.setAttribute(
        'sandbox',
        'allow-scripts allow-same-origin allow-forms allow-popups allow-top-navigation allow-popups-to-escape-sandbox'
      );
      this.officeFrameholder.nativeElement.appendChild(officeFrame);
      this.officeForm.nativeElement.action = this.actionUrl+"?access_token="+this.accessToken+"&access_token_ttl="+this.accessTokenTTL;
      this.officeForm.nativeElement.submit();
    

}
  
  getActionUrlForDocument() {
    var data = this.docid;
      this.msservice.getActionUrl(data).subscribe((data:any)=>
        {
          this.accessToken = data.access_token;
          this.accessTokenTTL = data.access_token_ttl;
          this.actionUrl = data.wopi_urlsrc;
          this.ViewDoc();
        });
  
  }

}

