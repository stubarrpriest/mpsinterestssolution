import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InterestsComponent } from './interests/interests.component';
import { InterestsDetailComponent } from './interests/interests-detail.component';


const routes: Routes = [
      { path: 'list', component: InterestsComponent },
      { path: 'detail/:identifier', component: InterestsDetailComponent},
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: '**', redirectTo: '/', pathMatch: 'full' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
