const API_URL = "http://localhost:5241";

export async function getVehicles() {
  const response = await fetch(`${API_URL}/api/vehicles`);

  if (!response.ok) {
    throw new Error("No se pudieron obtener los vehículos");
  }

  return response.json();
}

export async function getVehicleState(vehicleId: string) {
  const response = await fetch(
    `${API_URL}/api/vehicles/${vehicleId}/state`
  );

  if (!response.ok) {
    throw new Error("No se pudo obtener el estado del vehículo");
  }

  return response.json();
}

export async function getAlerts() {
  const response = await fetch(`${API_URL}/api/alerts`);

  if (!response.ok) {
    throw new Error("No se pudieron obtener las alertas");
  }

  return response.json();
}