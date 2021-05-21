import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SensorModule } from "../models/sensor-module";
import { environment } from "../../environments/environment";
import { SensorModuleConfig } from "../models/sensor-module-config";


@Injectable()
export class SensorModuleApiClient {
  constructor(private http: HttpClient) {
  }

  getSensorModules(): Observable<SensorModule[]> {
    return this.http.get<SensorModule[]>(environment.baseUrl + "/sensor-modules")
      .pipe(
        catchError(this.handleError)
      );
  }

  provisioningQueueGet(): Observable<SensorModuleConfig[]> {
    return this.http.get<SensorModuleConfig[]>(environment.baseUrl + "/sensor-modules/provisioning-queue")
      .pipe(
        catchError(this.handleError)
      );
  }

  provisioningQueueAdd(config: SensorModuleConfig): Observable<void | Object> {
    return this.http.post(environment.baseUrl + "/sensor-modules/provisioning-queue", config)
      .pipe(
        catchError(this.handleError)
      );
  }

  provisioningQueueDelete(serialNumber: string): Observable<void | Object> {
    return this.http.delete(environment.baseUrl + "/sensor-modules/provisioning-queue/" + serialNumber)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      const msg = 'An error occurred:' + error.error;
      console.error(msg);
      alert(msg);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      const msg =
        `Backend returned code ${error.status}, ` +
          `body was: ${error.error}`;
      console.error(msg);
      alert(msg);
    }

    // Return an observable with a user-facing error message.
    return throwError('Error with API call.');
  }
}
