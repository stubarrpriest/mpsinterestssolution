import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeGB from '@angular/common/locales/en-GB';

registerLocaleData(localeGB, 'en-GB');

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InterestsComponent } from './interests/interests.component';
import { HttpClientModule } from '@angular/common/http';
import { InterestsDetailComponent } from './interests/interests-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    InterestsComponent,
    InterestsDetailComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'en-GB' } 
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
