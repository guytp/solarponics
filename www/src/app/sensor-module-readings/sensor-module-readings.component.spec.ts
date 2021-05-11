import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorModuleReadingsComponent } from './sensor-module-readings.component';

describe('SensorModuleReadingsComponent', () => {
  let component: SensorModuleReadingsComponent;
  let fixture: ComponentFixture<SensorModuleReadingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorModuleReadingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorModuleReadingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
