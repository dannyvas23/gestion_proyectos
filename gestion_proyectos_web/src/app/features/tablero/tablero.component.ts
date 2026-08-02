import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tablero',
  standalone: true,
  template: `
    <div class="surface-card shadow-2 border-round p-4">
      <h1 class="text-2xl text-900 m-0 mb-3">
        <i class="pi pi-th-large mr-2 text-primary"></i>Tablero
      </h1>
      <p class="text-600 m-0">Tablero del proyecto {{ id }} (pendiente de implementar).</p>
    </div>
  `
})
export class TableroComponent implements OnInit {
  id: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
  }
}
