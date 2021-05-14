import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorModulesComponent } from './sensor-modules.component';

describe('SensorModulesComponent', () => {
  let component: SensorModulesComponent;
  let fixture: ComponentFixture<SensorModulesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorModulesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorModulesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
