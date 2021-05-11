import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { SensorReading } from "../models/sensor-reading";
import { environment } from "../../environments/environment";


@Injectable()
export class SensorReadingApiClient {
  constructor(private http: HttpClient) {
  }

  getSensorReadings(id: number, timeframe: string) {
    return this.http.get<SensorReading[]>(environment.baseUrl + "/readings/by-sensor-id/" + id + "/aggregate-by/" + timeframe)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    return throwError(
      'Something bad happened; please try again later.');
  }

}
