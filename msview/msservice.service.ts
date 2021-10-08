import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MsserviceService {
 
  readonly url = "api url";
    constructor(private http:HttpClient) { }

  getActionUrl(data: any) {
    return this.http.get(this.url + "/Home/Detail/"+ data+"?action=edit");
  }
}
