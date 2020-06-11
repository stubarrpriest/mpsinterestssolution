import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InterestsComponent } from './interests/interests.component';


const routes: Routes = [
      { path: 'list', component: InterestsComponent },
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
