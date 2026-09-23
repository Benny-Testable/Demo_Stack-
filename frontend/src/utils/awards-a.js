import { summariseApplications } from './awards-b.js';

export function summariseProgrammes(programmes) {
  return programmes.map((programme) => ({
    id: programme.id,
    code: programme.code,
    places: programme.placesAvailable ?? 0,
  }));
}

export function cycleTotals(programmes, applications) {
  return {
    programmes: summariseProgrammes(programmes).length,
    applications: summariseApplications(applications).length,
  };
}
