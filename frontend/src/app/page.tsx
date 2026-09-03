"use client";

import {
  Activity,
  AlertTriangle,
  Bell,
  Bot,
  Car,
  ChevronRight,
  Circle,
  Gauge,
  MapPin,
  Menu,
  Navigation,
  Settings,
  Truck,
  Wifi,
  X,
  Zap,
} from "lucide-react";
import { useEffect, useMemo, useState } from "react";
import {
  getAlerts,
  getVehicleState,
  getVehicles,
} from "@/lib/api";

type Vehicle = {
  id: string;
  vehicleId: string;
  name: string;
  status: number;
};

type VehicleState = {
  vehicleId: string;
  latitude: number;
  longitude: number;
  speed: number;
  lastTelemetryAt: string;
};

type Alert = {
  id: string;
  vehicleId: string;
  message: string;
  severity: number;
  createdAt: string;
  isResolved: boolean;
};

const vehicleStatus = {
  1: {
    label: "En movimiento",
    color: "text-[#00FFC2]",
    bg: "bg-[#00FFC2]/10",
    dot: "bg-[#00FFC2]",
  },
  2: {
    label: "Detenido",
    color: "text-[#F2B84B]",
    bg: "bg-[#F2B84B]/10",
    dot: "bg-[#F2B84B]",
  },
  3: {
    label: "Offline",
    color: "text-[#7C8794]",
    bg: "bg-[#7C8794]/10",
    dot: "bg-[#7C8794]",
  },
} as const;

const alertSeverity = {
  1: "Baja",
  2: "Media",
  3: "Alta",
  4: "Crítica",
} as const;

export default function Home() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [vehicleStates, setVehicleStates] = useState<
    Record<string, VehicleState>
  >({});
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [loading, setLoading] = useState(true);
  const [sidebarOpen, setSidebarOpen] = useState(false);

  useEffect(() => {
    async function loadDashboard() {
      try {
        setLoading(true);

        const vehicleData: Vehicle[] = await getVehicles();

        setVehicles(vehicleData);

        const states = await Promise.all(
          vehicleData.map(async (vehicle) => {
            try {
              const state = await getVehicleState(
                vehicle.vehicleId
              );

              return [vehicle.vehicleId, state] as const;
            } catch {
              return null;
            }
          })
        );

        const stateMap: Record<string, VehicleState> = {};

        for (const item of states) {
          if (item) {
            stateMap[item[0]] = item[1];
          }
        }

        setVehicleStates(stateMap);

        try {
          const alertData: Alert[] = await getAlerts();
          setAlerts(alertData);
        } catch {
          setAlerts([]);
        }
      } catch (error) {
        console.error(
          "Error cargando dashboard:",
          error
        );
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, []);

  const totalVehicles = vehicles.length;

  const movingVehicles = useMemo(
    () =>
      vehicles.filter(
        (vehicle) => vehicle.status === 1
      ).length,
    [vehicles]
  );

  const stoppedVehicles = useMemo(
    () =>
      vehicles.filter(
        (vehicle) => vehicle.status === 2
      ).length,
    [vehicles]
  );

  const offlineVehicles = useMemo(
    () =>
      vehicles.filter(
        (vehicle) => vehicle.status === 3
      ).length,
    [vehicles]
  );

  const activeAlerts = alerts.filter(
    (alert) => !alert.isResolved
  );

  return (
    <div className="min-h-screen bg-[#070B10] text-white">
      <div className="flex min-h-screen">

        {/* Sidebar */}
        <aside
          className={`
            fixed inset-y-0 left-0 z-50 w-64
            transform border-r border-white/[0.06]
            bg-[#05080C]
            transition-transform duration-200
            lg:static lg:translate-x-0
            ${
              sidebarOpen
                ? "translate-x-0"
                : "-translate-x-full"
            }
          `}
        >
          <div className="flex h-full flex-col">

            {/* Brand */}
            <div className="flex h-20 items-center justify-between border-b border-white/[0.06] px-6">
              <div>
                <div className="text-xl font-bold tracking-tight text-white">
                  FLEET
                  <span className="text-[#00FFC2]">
                    OPS
                  </span>
                </div>

                <div className="mt-1 text-[10px] uppercase tracking-[0.22em] text-[#7C8794]">
                  Mobility Intelligence
                </div>
              </div>

              <button
                onClick={() => setSidebarOpen(false)}
                className="text-[#A7B0BB] lg:hidden"
              >
                <X size={20} />
              </button>
            </div>

            {/* Navigation */}
            <nav className="flex-1 px-3 py-6">

              <div className="mb-3 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-[#596572]">
                Operación
              </div>

              <NavItem
                icon={<Activity size={18} />}
                label="Dashboard"
                active
              />

              <NavItem
                icon={<Truck size={18} />}
                label="Vehículos"
              />

              <NavItem
                icon={<Navigation size={18} />}
                label="Telemetría"
              />

              <NavItem
                icon={<Bell size={18} />}
                label="Alertas"
                badge={
                  activeAlerts.length > 0
                    ? activeAlerts.length
                    : undefined
                }
              />

              <div className="mb-3 mt-8 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-[#596572]">
                Inteligencia
              </div>

              <NavItem
                icon={<Bot size={18} />}
                label="SIMON AI"
              />

              <div className="mb-3 mt-8 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-[#596572]">
                Sistema
              </div>

              <NavItem
                icon={<Settings size={18} />}
                label="Configuración"
              />

            </nav>

            {/* System */}
            <div className="border-t border-white/[0.06] p-4">

              <div className="rounded-xl border border-[#00FFC2]/10 bg-[#00FFC2]/[0.04] p-4">

                <div className="mb-2 flex items-center gap-2">

                  <span className="h-2 w-2 animate-pulse rounded-full bg-[#00FFC2]" />

                  <span className="text-xs font-medium text-white">
                    Sistema operativo
                  </span>

                </div>

                <div className="text-[11px] text-[#697582]">
                  Kafka • TimescaleDB • API
                </div>

              </div>
            </div>

          </div>
        </aside>

        {/* Main */}
        <main className="min-w-0 flex-1">

          {/* Header */}
          <header className="flex h-20 items-center justify-between border-b border-white/[0.06] bg-[#080C11]/90 px-4 backdrop-blur md:px-8">

            <div className="flex items-center gap-4">

              <button
                onClick={() => setSidebarOpen(true)}
                className="text-[#00FFC2] lg:hidden"
              >
                <Menu size={22} />
              </button>

              <div>

                <div className="flex items-center gap-2">

                  <h1 className="text-lg font-semibold text-white">
                    Centro de Operaciones
                  </h1>

                  <span className="hidden rounded-full border border-[#00FFC2]/20 bg-[#00FFC2]/10 px-2 py-1 text-[10px] font-semibold text-[#00FFC2] sm:block">
                    LIVE
                  </span>

                </div>

                <p className="text-xs text-[#71808C]">
                  Monitoreo de flota en tiempo real
                </p>

              </div>
            </div>

            <div className="flex items-center gap-4">

              <div className="hidden text-right sm:block">

                <div className="text-xs font-medium text-[#D7DEE4]">
                  Operador
                </div>

                <div className="text-[10px] text-[#65717D]">
                  Fleet Control
                </div>

              </div>

              <div className="flex h-9 w-9 items-center justify-center rounded-full border border-[#00FFC2]/20 bg-[#00FFC2]/10 text-xs font-bold text-[#00FFC2]">
                FC
              </div>

            </div>

          </header>

          <div className="space-y-6 p-4 md:p-8">

            {/* KPIs */}
            <section className="grid grid-cols-2 gap-4 xl:grid-cols-4">

              <KpiCard
                title="Vehículos"
                value={totalVehicles}
                subtitle="Flota registrada"
                icon={<Truck size={20} />}
              />

              <KpiCard
                title="En movimiento"
                value={movingVehicles}
                subtitle="Vehículos activos"
                icon={<Navigation size={20} />}
                accent
              />

              <KpiCard
                title="Detenidos"
                value={stoppedVehicles}
                subtitle="Requieren atención"
                icon={<Circle size={20} />}
                warning={stoppedVehicles > 0}
              />

              <KpiCard
                title="Offline"
                value={offlineVehicles}
                subtitle="Sin conexión"
                icon={<Wifi size={20} />}
              />

            </section>

            {/* Map + Vehicles */}
            <section className="grid gap-6 xl:grid-cols-[1fr_380px]">

              {/* Map */}
              <div className="overflow-hidden rounded-2xl border border-white/[0.07] bg-[#0C1117] shadow-2xl">

                <div className="flex items-center justify-between border-b border-white/[0.06] px-5 py-4">

                  <div>
                    <h2 className="text-sm font-semibold text-white">
                      Mapa operacional
                    </h2>

                    <p className="mt-1 text-[11px] text-[#687582]">
                      Posiciones reportadas por la flota
                    </p>
                  </div>

                  <div className="flex items-center gap-2 text-[10px] text-[#00FFC2]">

                    <span className="h-2 w-2 animate-pulse rounded-full bg-[#00FFC2]" />

                    Tiempo real

                  </div>

                </div>

                <div className="relative h-[430px] overflow-hidden bg-[#071019]">

                  {/* Grid */}
                  <div
                    className="absolute inset-0 opacity-40"
                    style={{
                      backgroundImage:
                        "linear-gradient(rgba(0,255,194,.06) 1px, transparent 1px), linear-gradient(90deg, rgba(0,255,194,.06) 1px, transparent 1px)",
                      backgroundSize: "50px 50px",
                    }}
                  />

                  {/* Roads */}
                  <div className="absolute left-[5%] top-[42%] h-[2px] w-[90%] rotate-[-12deg] rounded-full bg-[#25333D]" />

                  <div className="absolute left-[20%] top-[20%] h-[2px] w-[70%] rotate-[32deg] rounded-full bg-[#25333D]" />

                  <div className="absolute left-[48%] top-[5%] h-[2px] w-[90%] rotate-[10deg] rounded-full bg-[#25333D]" />

                  <div className="absolute left-[15%] top-[60%] h-[2px] w-[70%] rotate-[18deg] rounded-full bg-[#25333D]" />

                  {/* Operational area */}
                  <div className="absolute left-[25%] top-[25%] h-40 w-72 rounded-[40%] border border-[#00FFC2]/10 bg-[#00FFC2]/[0.025]" />

                  {/* Vehicle markers */}
                  {vehicles.map(
                    (vehicle, index) => {
                      const state =
                        vehicleStates[
                          vehicle.vehicleId
                        ];

                      return (
                        <VehicleMarker
                          key={vehicle.id}
                          vehicle={vehicle}
                          state={state}
                          index={index}
                        />
                      );
                    }
                  )}

                  {vehicles.length === 0 &&
                    !loading && (
                      <div className="absolute inset-0 flex items-center justify-center">

                        <div className="text-center">

                          <MapPin
                            size={30}
                            className="mx-auto mb-3 text-[#53616D]"
                          />

                          <p className="text-sm text-[#78848E]">
                            Sin vehículos disponibles
                          </p>

                        </div>

                      </div>
                    )}

                  {/* Map controls */}
                  <div className="absolute right-4 top-4 overflow-hidden rounded-lg border border-white/[0.08] bg-[#0C1117] shadow-xl">

                    <button className="flex h-9 w-9 items-center justify-center border-b border-white/[0.06] text-[#00FFC2] hover:bg-white/[0.04]">
                      +
                    </button>

                    <button className="flex h-9 w-9 items-center justify-center text-[#00FFC2] hover:bg-white/[0.04]">
                      −
                    </button>

                  </div>

                  {/* Legend */}
                  <div className="absolute bottom-4 left-4 rounded-lg border border-white/[0.08] bg-[#0C1117]/95 px-4 py-3 shadow-xl">

                    <div className="mb-2 text-[9px] font-semibold uppercase tracking-widest text-[#65717D]">
                      Estado
                    </div>

                    <div className="flex gap-4 text-[10px] text-[#89949E]">

                      <Legend
                        color="bg-[#00FFC2]"
                        label="Movimiento"
                      />

                      <Legend
                        color="bg-[#F2B84B]"
                        label="Detenido"
                      />

                      <Legend
                        color="bg-[#7C8794]"
                        label="Offline"
                      />

                    </div>

                  </div>

                </div>
              </div>

              {/* Fleet status */}
              <div className="rounded-2xl border border-white/[0.07] bg-[#0C1117] shadow-2xl">

                <div className="flex items-center justify-between border-b border-white/[0.06] px-5 py-4">

                  <div>

                    <h2 className="text-sm font-semibold text-white">
                      Estado de la flota
                    </h2>

                    <p className="mt-1 text-[11px] text-[#687582]">
                      Vehículos monitoreados
                    </p>

                  </div>

                  <span className="text-[11px] text-[#687582]">
                    {totalVehicles} total
                  </span>

                </div>

                <div className="divide-y divide-white/[0.05]">

                  {loading ? (
                    <LoadingRows />
                  ) : (
                    vehicles.map(
                      (vehicle) => (
                        <VehicleRow
                          key={vehicle.id}
                          vehicle={vehicle}
                          state={
                            vehicleStates[
                              vehicle.vehicleId
                            ]
                          }
                        />
                      )
                    )
                  )}

                </div>

                {vehicles.length > 0 && (
                  <button className="flex w-full items-center justify-center gap-2 border-t border-white/[0.06] py-4 text-xs font-medium text-[#00FFC2] hover:bg-white/[0.025]">
                    Ver todos los vehículos
                    <ChevronRight size={14} />
                  </button>
                )}

              </div>
            </section>

            {/* Alerts + AI */}
            <section className="grid gap-6 lg:grid-cols-2">

              {/* Alerts */}
              <div className="rounded-2xl border border-white/[0.07] bg-[#0C1117] shadow-2xl">

                <div className="flex items-center justify-between border-b border-white/[0.06] px-5 py-4">

                  <div>

                    <h2 className="text-sm font-semibold text-white">
                      Alertas operacionales
                    </h2>

                    <p className="mt-1 text-[11px] text-[#687582]">
                      Eventos que requieren atención
                    </p>

                  </div>

                  <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#D94A4A]/10 text-[#FF6B6B]">
                    <AlertTriangle size={16} />
                  </div>

                </div>

                <div className="p-4">

                  {activeAlerts.length === 0 ? (

                    <div className="flex items-center gap-4 rounded-xl border border-[#00FFC2]/10 bg-[#00FFC2]/[0.035] p-4">

                      <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-[#00FFC2]/10">

                        <Zap
                          size={17}
                          className="text-[#00FFC2]"
                        />

                      </div>

                      <div>

                        <div className="text-xs font-medium text-[#D7DEE4]">
                          Operación normal
                        </div>

                        <div className="mt-1 text-[10px] text-[#687582]">
                          No existen alertas activas.
                        </div>

                      </div>

                    </div>

                  ) : (

                    <div className="space-y-2">

                      {activeAlerts
                        .slice(0, 4)
                        .map((alert) => (
                          <AlertRow
                            key={alert.id}
                            alert={alert}
                          />
                        ))}

                    </div>

                  )}

                </div>
              </div>

              {/* SIMON AI */}
              <div className="relative overflow-hidden rounded-2xl border border-[#00FFC2]/15 bg-gradient-to-br from-[#0C1918] via-[#0C1117] to-[#091219] shadow-2xl">

                <div className="absolute -right-20 -top-20 h-48 w-48 rounded-full bg-[#00FFC2]/10 blur-3xl" />

                <div className="relative p-6">

                  <div className="mb-5 flex items-center justify-between">

                    <div className="flex items-center gap-3">

                      <div className="flex h-10 w-10 items-center justify-center rounded-xl border border-[#00FFC2]/10 bg-[#00FFC2]/10 text-[#00FFC2]">
                        <Bot size={21} />
                      </div>

                      <div>

                        <div className="text-sm font-semibold text-white">
                          SIMON AI
                        </div>

                        <div className="text-[10px] text-[#00FFC2]">
                          Operational Intelligence
                        </div>

                      </div>

                    </div>

                    <span className="rounded-full border border-[#00FFC2]/20 bg-[#00FFC2]/10 px-2 py-1 text-[9px] font-medium text-[#00FFC2]">
                      AI ONLINE
                    </span>

                  </div>

                  <p className="max-w-xl text-sm leading-6 text-[#A7B0BB]">
                    Consulta el estado operacional de tu
                    flota utilizando lenguaje natural.
                  </p>

                  <div className="mt-5 flex items-center gap-3 rounded-xl border border-white/[0.07] bg-[#080D12] px-4 py-3">

                    <Bot
                      size={15}
                      className="shrink-0 text-[#00FFC2]"
                    />

                    <span className="flex-1 text-[11px] text-[#697582]">
                      ¿Qué vehículos llevan más de 20
                      minutos detenidos?
                    </span>

                    <ChevronRight
                      size={15}
                      className="text-[#00FFC2]"
                    />

                  </div>

                </div>
              </div>

            </section>

          </div>
        </main>
      </div>
    </div>
  );
}

/* --------------------------------
   Navigation
-------------------------------- */

function NavItem({
  icon,
  label,
  active = false,
  badge,
}: {
  icon: React.ReactNode;
  label: string;
  active?: boolean;
  badge?: number;
}) {
  return (
    <button
      className={`
        mb-1 flex w-full items-center gap-3 rounded-lg
        px-3 py-2.5 text-sm transition
        ${
          active
            ? "border border-[#00FFC2]/10 bg-[#00FFC2]/10 text-[#00FFC2]"
            : "text-[#8D99A5] hover:bg-white/[0.04] hover:text-white"
        }
      `}
    >
      {icon}

      <span className="flex-1 text-left">
        {label}
      </span>

      {badge !== undefined && (
        <span className="rounded-full bg-[#D94A4A] px-2 py-0.5 text-[9px] text-white">
          {badge}
        </span>
      )}
    </button>
  );
}

/* --------------------------------
   KPI
-------------------------------- */

function KpiCard({
  title,
  value,
  subtitle,
  icon,
  accent = false,
  warning = false,
}: {
  title: string;
  value: number;
  subtitle: string;
  icon: React.ReactNode;
  accent?: boolean;
  warning?: boolean;
}) {
  return (
    <div className="rounded-2xl border border-white/[0.07] bg-[#0C1117] p-5 shadow-xl">

      <div className="flex items-start justify-between">

        <div>

          <div className="text-[10px] font-medium uppercase tracking-wider text-[#687582]">
            {title}
          </div>

          <div className="mt-2 text-3xl font-semibold tracking-tight text-white">
            {value}
          </div>

          <div className="mt-1 text-[10px] text-[#5F6B76]">
            {subtitle}
          </div>

        </div>

        <div
          className={`
            flex h-9 w-9 items-center justify-center rounded-lg
            ${
              warning
                ? "bg-[#F2B84B]/10 text-[#F2B84B]"
                : accent
                  ? "bg-[#00FFC2]/10 text-[#00FFC2]"
                  : "bg-white/[0.05] text-[#8C98A4]"
            }
          `}
        >
          {icon}
        </div>

      </div>
    </div>
  );
}

/* --------------------------------
   Vehicle Marker
-------------------------------- */

function VehicleMarker({
  vehicle,
  state,
  index,
}: {
  vehicle: Vehicle;
  state?: VehicleState;
  index: number;
}) {
  const positions = [
    { left: "20%", top: "35%" },
    { left: "62%", top: "25%" },
    { left: "75%", top: "58%" },
    { left: "42%", top: "68%" },
    { left: "30%", top: "52%" },
  ];

  const position =
    positions[index % positions.length];

  return (
    <div
      className="absolute"
      style={{
        left: position.left,
        top: position.top,
      }}
    >
      <div className="group relative">

        <div
          className={`
            flex h-10 w-10 items-center justify-center
            rounded-full border bg-[#0C1117] shadow-lg
            ${
              vehicle.status === 1
                ? "border-[#00FFC2]/60 text-[#00FFC2]"
                : vehicle.status === 2
                  ? "border-[#F2B84B]/60 text-[#F2B84B]"
                  : "border-[#7C8794]/50 text-[#7C8794]"
            }
          `}
        >
          <Car size={16} />
        </div>

        {vehicle.status === 1 && (
          <span className="absolute inset-0 -z-10 animate-ping rounded-full bg-[#00FFC2]/10" />
        )}

        <div className="absolute left-1/2 top-full z-20 mt-2 hidden -translate-x-1/2 whitespace-nowrap rounded-lg border border-white/[0.08] bg-[#0C1117] px-3 py-2 shadow-2xl group-hover:block">

          <div className="text-[10px] font-semibold text-white">
            {vehicle.vehicleId}
          </div>

          <div className="mt-1 text-[9px] text-[#687582]">
            {state
              ? `${state.speed} km/h`
              : vehicleStatus[
                  vehicle.status as keyof typeof vehicleStatus
                ]?.label}
          </div>

        </div>

      </div>
    </div>
  );
}

/* --------------------------------
   Vehicle Row
-------------------------------- */

function VehicleRow({
  vehicle,
  state,
}: {
  vehicle: Vehicle;
  state?: VehicleState;
}) {
  const status =
    vehicleStatus[
      vehicle.status as keyof typeof vehicleStatus
    ] ?? vehicleStatus[3];

  return (
    <div className="flex items-center gap-3 px-5 py-4 hover:bg-white/[0.02]">

      <div
        className={`
          flex h-9 w-9 shrink-0 items-center justify-center
          rounded-lg ${status.bg}
        `}
      >
        <Truck
          size={17}
          className={status.color}
        />
      </div>

      <div className="min-w-0 flex-1">

        <div className="text-xs font-medium text-[#D7DEE4]">
          {vehicle.vehicleId}
        </div>

        <div className="mt-1 truncate text-[10px] text-[#697582]">
          {vehicle.name}
        </div>

      </div>

      <div className="text-right">

        <div
          className={`flex items-center justify-end gap-1.5 text-[10px] ${status.color}`}
        >

          <span
            className={`h-1.5 w-1.5 rounded-full ${status.dot}`}
          />

          {status.label}

        </div>

        {state && (
          <div className="mt-1 flex items-center justify-end gap-1 text-[9px] text-[#697582]">
            <Gauge size={10} />
            {state.speed} km/h
          </div>
        )}

      </div>
    </div>
  );
}

/* --------------------------------
   Alert
-------------------------------- */

function AlertRow({ alert }: { alert: Alert }) {
  const severity =
    alertSeverity[
      alert.severity as keyof typeof alertSeverity
    ] ?? "Media";

  return (
    <div className="flex items-center gap-3 rounded-xl border border-white/[0.06] bg-[#080D12] p-3">

      <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#D94A4A]/10 text-[#FF6B6B]">
        <AlertTriangle size={14} />
      </div>

      <div className="min-w-0 flex-1">

        <div className="text-[10px] font-medium text-[#D7DEE4]">
          {alert.vehicleId}
        </div>

        <div className="mt-1 truncate text-[10px] text-[#697582]">
          {alert.message}
        </div>

      </div>

      <span className="text-[9px] text-[#FF6B6B]">
        {severity}
      </span>

    </div>
  );
}

/* --------------------------------
   Legend
-------------------------------- */

function Legend({
  color,
  label,
}: {
  color: string;
  label: string;
}) {
  return (
    <div className="flex items-center gap-1.5">

      <span
        className={`h-1.5 w-1.5 rounded-full ${color}`}
      />

      {label}

    </div>
  );
}

/* --------------------------------
   Loading
-------------------------------- */

function LoadingRows() {
  return (
    <>
      {[1, 2, 3, 4, 5].map((item) => (
        <div
          key={item}
          className="flex items-center gap-3 px-5 py-4"
        >
          <div className="h-9 w-9 animate-pulse rounded-lg bg-white/[0.05]" />

          <div className="flex-1">

            <div className="h-3 w-24 animate-pulse rounded bg-white/[0.05]" />

            <div className="mt-2 h-2 w-16 animate-pulse rounded bg-white/[0.05]" />

          </div>

          <div className="h-3 w-20 animate-pulse rounded bg-white/[0.05]" />
        </div>
      ))}
    </>
  );
}