import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StyleMaterialModule } from '../style-material/style-material.module';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [RouterModule, StyleMaterialModule],
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
