import React, { useState, useEffect } from "react";
import {
  Monitor,
  Settings,
  Trash2,
  Cpu,
  HardDrive,
  MemoryStick,
  Shield,
  Calendar,
  DownloadCloud,
  Eye,
  Moon,
  Battery,
  Check,
  Sparkles,
  Activity,
  X,
  Github,
  Wifi,
  WifiOff,
  CheckCircle,
  Loader,
  Server,
  Watch,
  Zap,
  RotateCcw,
} from "lucide-react";
import { User } from "lucide-react";

export default function HSTWindowsUtility() {
  const [apiStatus, setApiStatus] = useState("CHECKING");
  const [currentStatus, setCurrentStatus] = useState(
    "READY - SELECT AN OPERATION"
  );
  const [activeOperation, setActiveOperation] = useState(null);
  const [systemInfo, setSystemInfo] = useState({
    user: "Loading...",
    time: "Loading...",
    gpu: "Loading...",
    cpu: "Loading...",
    ram: "Loading...",
    storage: "Loading...",
  });

  const [serviceOptions, setServiceOptions] = useState({
    recommended: false,
    bluetooth: false,
    hyperv: false,
    xbox: false,
  });

  const [debloatOptions, setDebloatOptions] = useState({
    msApps: false,
    edge: false,
    onedrive: false,
    xboxApps: false,
    storeApps: false,
  });

  const [cleanupOptions, setCleanupOptions] = useState({
    temp: false,
    cache: false,
    eventLogs: false,
    powerPlans: false,
  });

  const [revertOptions, setRevertOptions] = useState({
    service: false,
    task: false,
    wUpdate: false,
    registry: false,
  });

  useEffect(() => {
    checkApiStatus();
    fetchSystemInfo();
  }, []);

  const checkApiStatus = async () => {
    try {
      const response = await fetch("http://localhost:5200/api/system/test");
      setApiStatus(response.ok ? "online" : "offline");
    } catch (error) {
      setApiStatus("offline");
    }
  };

  const fetchSystemInfo = async () => {
    try {
      const response = await fetch("http://localhost:5200/api/system/sysinfo");

      if (response.ok) {
        const data = await response.json();
        setSystemInfo(data);
      } else {
        setSystemInfo({
          user: "Error",
          time: "Error",
          gpu: "Error",
          cpu: "Error",
          ram: "Error",
          storage: "Error",
        });
      }
    } catch (error) {
      setSystemInfo({
        user: "Offline",
        time: "Offline",
        gpu: "Offline",
        cpu: "Offline",
        ram: "Offline",
        storage: "Offline",
      });
    }
  };

  const executeApiCall = async (endpoint, message, body = null) => {
    if (apiStatus !== "online") {
      setCurrentStatus("API OFFLINE");
      return;
    }

    setActiveOperation(endpoint);
    setCurrentStatus(message);

    try {
      const options = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Accept: "application/json",
        },
      };

      if (body) {
        options.body = JSON.stringify(body);
      }

      const response = await fetch(
        `http://localhost:5200/api/system/${endpoint}`,
        options
      );

      const result = await response.json();
      setCurrentStatus(result.status || "OPERATION COMPLETED");
    } catch (error) {
      console.error("API Error:", error);
      setCurrentStatus(`NETWORK ERROR: ${error.message}`);
    }

    setTimeout(() => {
      setCurrentStatus("READY - SELECT AN OPERATION");
      setActiveOperation(null);
    }, 3000);
  };

  const NeonButton = ({
    children,
    onClick,
    icon: Icon,
    gradient,
    textColor = "#ffffff",
    disabled = false,
  }) => (
    <button
      onClick={onClick}
      disabled={disabled || activeOperation !== null}
      style={{
        width: "100%",
        height: "38px",
        background:
          gradient || "linear-gradient(135deg, #0a0a0a 0%, #000000 100%)",
        border: "none",
        borderRadius: "8px",
        color: textColor,
        fontSize: "11px",
        fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
        fontWeight: "700",
        cursor: disabled || activeOperation ? "not-allowed" : "pointer",
        transition: "all 0.2s ease",
        textTransform: "uppercase",
        letterSpacing: "0.8px",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: "8px",
        position: "relative",
        opacity: disabled || activeOperation ? 0.5 : 1,
        boxShadow: "0 2px 10px rgba(0, 0, 0, 0.3)",
      }}
      onMouseEnter={(e) => {
        if (!disabled && !activeOperation) {
          e.currentTarget.style.transform = "translateY(-1px)";
          e.currentTarget.style.boxShadow = "0 4px 20px rgba(0, 0, 0, 0.5)";
        }
      }}
      onMouseLeave={(e) => {
        if (!disabled && !activeOperation) {
          e.currentTarget.style.transform = "translateY(0)";
          e.currentTarget.style.boxShadow = "0 2px 10px rgba(0, 0, 0, 0.3)";
        }
      }}
    >
      <Icon style={{ width: "15px", height: "15px", color: textColor }} />
      <span>{children}</span>
    </button>
  );

  const NeonCheckbox = ({ checked, onChange, label, color = "#ffffff" }) => (
    <label
      style={{
        display: "flex",
        alignItems: "center",
        gap: "6px",
        cursor: "pointer",
        padding: "4px 8px",
        borderRadius: "6px",
        transition: "all 0.2s ease",
        background: checked ? "rgba(255, 255, 255, 0.05)" : "transparent",
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.background = checked
          ? "rgba(255, 255, 255, 0.08)"
          : "rgba(255, 255, 255, 0.02)";
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.background = checked
          ? "rgba(255, 255, 255, 0.05)"
          : "transparent";
      }}
    >
      <div
        style={{
          width: "16px",
          height: "16px",
          borderRadius: "4px",
          border: `2px solid ${checked ? color : "#444444"}`,
          background: checked ? color : "transparent",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          transition: "all 0.2s ease",
        }}
      >
        {checked && (
          <Check
            style={{
              width: "10px",
              height: "10px",
              color: "black",
              strokeWidth: 3,
            }}
          />
        )}
      </div>
      <input
        type="checkbox"
        checked={checked}
        onChange={onChange}
        style={{ display: "none" }}
      />
      <span
        style={{
          color: "#cccccc",
          fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
          fontWeight: "600",
          fontSize: "12px",
        }}
      >
        {label}
      </span>
    </label>
  );

  const OptionsPanel = ({
    title,
    options,
    setOptions,
    onExecute,
    color,
    gradient,
    icon: Icon,
  }) => {
    const optionLabels = {
      recommended: "HST Essentials",
      bluetooth: "Bluetooth",
      hyperv: "Hyper-V",
      xbox: "Xbox",
      msApps: "Microsoft Apps",
      edge: "Microsoft Edge",
      onedrive: "OneDrive",
      xboxApps: "Xbox Apps",
      storeApps: "Microsoft Store",
      temp: "Temporary Files",
      cache: "Browser Cache",
      eventLogs: "Event Logs",
      powerPlans: "Default Powerplans",
      service: "Services",
      task: "Task Scheduler",
      wUpdate: "Windows Update",
      registry: "Registry",
    };

    return (
      <div
        style={{
          background: "#0a0a0a",
          border: "1px solid #1a1a1a",
          borderRadius: "12px",
          padding: "14px",
          boxShadow: "0 4px 12px rgba(0, 0, 0, 0.5)",
          transition: "all 0.2s ease",
          height: "230px",
          display: "flex",
          flexDirection: "column",
        }}
        onMouseEnter={(e) => {
          e.currentTarget.style.borderColor = "#2a2a2a";
          e.currentTarget.style.transform = "translateY(-1px)";
        }}
        onMouseLeave={(e) => {
          e.currentTarget.style.borderColor = "#1a1a1a";
          e.currentTarget.style.transform = "translateY(0)";
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            gap: "8px",
            marginBottom: "8px",
          }}
        >
          <Icon style={{ width: "16px", height: "16px", color }} />
          <h3
            style={{
              color: "#f3f4f6",
              fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
              fontWeight: "700",
              fontSize: "13px",
              textTransform: "uppercase",
              letterSpacing: "0.5px",
              margin: 0,
            }}
          >
            {title}
          </h3>
        </div>

        <div style={{ marginBottom: "4px", flex: 1 }}>
          {Object.entries(options).map(([key, value]) => (
            <div key={key} style={{ marginBottom: "4px" }}>
              <NeonCheckbox
                checked={value}
                onChange={(e) =>
                  setOptions((prev) => ({ ...prev, [key]: e.target.checked }))
                }
                label={optionLabels[key] || key.toUpperCase()}
                color={color}
              />
            </div>
          ))}
        </div>

        <NeonButton onClick={onExecute} gradient={gradient} icon={Icon} textColor={color}>
          Execute
        </NeonButton>
      </div>
    );
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #000000 0%, #0a0a0a 100%)",
        fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
        position: "relative",
        overflow: "hidden",
      }}
    >
      {/* Animated grid background */}
      <div
        style={{
          position: "fixed",
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          backgroundImage: `
          linear-gradient(rgba(255, 255, 255, 0.02) 1px, transparent 1px),
          linear-gradient(90deg, rgba(255, 255, 255, 0.02) 1px, transparent 1px)
        `,
          backgroundSize: "50px 50px",
          animation: "grid-move 20s linear infinite",
          zIndex: 0,
        }}
      />

      <style>{`
        @keyframes grid-move {
          0% { transform: translate(0, 0); }
          100% { transform: translate(50px, 50px); }
        }
      `}</style>

      {/* Exit Button */}
      <button
        onClick={() => window.close()}
        style={{
          position: "fixed",
          top: "20px",
          right: "20px",
          zIndex: 1000,
          width: "36px",
          height: "36px",
          background: "#0a0a0a",
          border: "1px solid #1a1a1a",
          borderRadius: "50%",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          cursor: "pointer",
          transition: "all 0.2s ease",
          boxShadow: "0 2px 8px rgba(0, 0, 0, 0.5)",
        }}
        onMouseEnter={(e) => {
          e.currentTarget.style.background = "#1a1a1a";
          e.currentTarget.style.borderColor = "#2a2a2a";
        }}
        onMouseLeave={(e) => {
          e.currentTarget.style.background = "#0a0a0a";
          e.currentTarget.style.borderColor = "#1a1a1a";
        }}
      >
        <X style={{ width: "18px", height: "18px", color: "#666666" }} />
      </button>

      <div
        style={{
          maxWidth: "1000px",
          margin: "0 auto",
          padding: "25px 20px",
          position: "relative",
          zIndex: 1,
        }}
      >
        {/* Header */}
        <div style={{ textAlign: "center", marginBottom: "25px" }}>
          <p
            style={{
              color: "#e5e7eb",
              fontSize: "33px",
              fontWeight: "700",
              marginBottom: "4px",
              letterSpacing: "1px",
              textTransform: "uppercase",
            }}
          >
            HST Windows Utility
          </p>
          <p
            style={{
              color: "#9ca3af",
              fontSize: "11px",
              fontWeight: "600",
              letterSpacing: "0.5px",
              textTransform: "uppercase",
            }}
          >
            Create Restore Point Before Modifications
          </p>

          {/* API Status Badge */}
          <div
            style={{
              display: "inline-flex",
              alignItems: "center",
              gap: "6px",
              padding: "6px 12px",
              background:
                apiStatus === "online"
                  ? "rgba(139, 92, 246, 0.15)"
                  : "rgba(239, 68, 68, 0.1)",
              borderRadius: "16px",
              border: `1px solid ${
                apiStatus === "online" ? "#8b5cf6" : "#ef4444"
              }`,
              marginTop: "16px",
            }}
          >
            {apiStatus === "online" ? (
              <Wifi
                style={{ width: "14px", height: "14px", color: "#8b5cf6" }}
              />
            ) : (
              <WifiOff
                style={{ width: "14px", height: "14px", color: "#ef4444" }}
              />
            )}
            <span
              style={{
                fontSize: "11px",
                fontWeight: "700",
                letterSpacing: "0.5px",
                color: apiStatus === "online" ? "#8b5cf6" : "#ef4444",
                textTransform: "uppercase",
              }}
            >
              API {apiStatus}
            </span>
          </div>

          {/* Status Bar */}
          <div
            style={{
              background: "#0a0a0a",
              border: "1px solid #1a1a1a",
              borderRadius: "8px",
              padding: "12px 16px",
              marginTop: "16px",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.5)",
            }}
          >
            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                gap: "8px",
              }}
            >
              {activeOperation ? (
                <Loader
                  style={{ width: "16px", height: "16px", color: "#8b5cf6" }}
                  className="animate-spin"
                />
              ) : (
                <CheckCircle
                  style={{ width: "16px", height: "16px", color: "#8b5cf6" }}
                />
              )}
              <span
                style={{
                  color: "#e5e7eb",
                  fontWeight: "600",
                  fontSize: "12px",
                  letterSpacing: "0.3px",
                }}
              >
                {currentStatus}
              </span>
            </div>
          </div>
        </div>

        {/* Main Content Grid */}
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "1.7fr 240px",
            gap: "16px",
            marginBottom: "16px",
          }}
        >
          {/* Left - System Info Panel */}
          <div
            style={{
              background: "#0a0a0a",
              border: "1px solid #1a1a1a",
              borderRadius: "12px",
              padding: "18px",
              boxShadow: "0 4px 12px rgba(0, 0, 0, 0.5)",
              display: "flex",
              flexDirection: "column",
            }}
          >
            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                gap: "8px",
                marginBottom: "16px",
              }}
            >
              <Server
                style={{ width: "18px", height: "18px", color: "#8b5cf6" }}
              />
              <h2
                style={{
                  color: "#f3f4f6",
                  fontSize: "14px",
                  fontWeight: "700",
                  textTransform: "uppercase",
                  letterSpacing: "0.5px",
                  margin: 0,
                }}
              >
                System Information
              </h2>
            </div>

            <div
              style={{
                display: "grid",
                gridTemplateColumns: "repeat(2, 1fr)",
                gap: "18px",
                marginBottom: "16px",
                flex: 1,
                alignContent: "stretch",
                gridAutoRows: "1fr",
              }}
            >
              {[
                {
                  label: "USER",
                  value: systemInfo.user,
                  icon: User,
                  color: "#8b5cf6",
                },
                {
                  label: "TIME",
                  value: systemInfo.time,
                  icon: Watch,
                  color: "#8b5cf6",
                },
                {
                  label: "GPU",
                  value: systemInfo.gpu,
                  icon: Monitor,
                  color: "#8b5cf6",
                },
                {
                  label: "CPU",
                  value: systemInfo.cpu,
                  icon: Cpu,
                  color: "#c084fc",
                },
                {
                  label: "RAM",
                  value: systemInfo.ram,
                  icon: MemoryStick,
                  color: "#c084fc",
                },
                {
                  label: "Storage",
                  value: systemInfo.storage,
                  icon: HardDrive,
                  color: "#c084fc",
                },
              ].map((item, i) => (
                <div
                  key={i}
                  style={{
                    background: "#000000",
                    border: `1px solid #1a1a1a`,
                    borderRadius: "8px",
                    padding: "10px",
                    transition: "all 0.2s ease",
                    cursor: "pointer",
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "center",
                    textAlign: "center",
                  }}
                  onMouseEnter={(e) => {
                    e.currentTarget.style.borderColor = "#333333";
                    e.currentTarget.style.background = "#0a0a0a";
                  }}
                  onMouseLeave={(e) => {
                    e.currentTarget.style.borderColor = "#1a1a1a";
                    e.currentTarget.style.background = "#000000";
                  }}
                >
                  <div
                    style={{
                      display: "flex",
                      alignItems: "center",
                      gap: "6px",
                      marginBottom: "4px",
                    }}
                  >
                    <item.icon
                      style={{
                        width: "14px",
                        height: "14px",
                        color: item.color,
                      }}
                    />
                    <div
                      style={{
                        color: "#9ca3af",
                        fontSize: "10px",
                        textTransform: "uppercase",
                        letterSpacing: "0.5px",
                        fontWeight: "700",
                      }}
                    >
                      {item.label}
                    </div>
                  </div>
                  <div
                    style={{
                      color: "#f3f4f6",
                      fontWeight: "600",
                      fontSize: "12px",
                      whiteSpace: "nowrap",
                      overflow: "hidden",
                      textOverflow: "ellipsis",
                      width: "100%",
                    }}
                  >
                    {item.value}
                  </div>
                </div>
              ))}
            </div>

            <div style={{ textAlign: "center" }}>
              <a
                href="https://github.com/hselimt"
                target="_blank"
                rel="noopener noreferrer"
                style={{
                  display: "inline-flex",
                  alignItems: "center",
                  gap: "6px",
                  padding: "7px 14px",
                  background: "#000000",
                  border: "1px solid #1a1a1a",
                  borderRadius: "6px",
                  color: "#ffffff",
                  textDecoration: "none",
                  fontWeight: "700",
                  fontSize: "11px",
                  letterSpacing: "0.5px",
                  transition: "all 0.2s ease",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.borderColor = "#333333";
                  e.currentTarget.style.color = "#cccccc";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.borderColor = "#1a1a1a";
                  e.currentTarget.style.color = "#ffffff";
                }}
              >
                <Github style={{ width: "14px", height: "14px" }} />
                GITHUB
              </a>
            </div>
          </div>

          {/* Right - 8 Operation Buttons */}
          <div
            style={{ display: "grid", gridTemplateColumns: "1fr", gap: "7px" }}
          >
            <NeonButton
              onClick={() =>
                executeApiCall("restore-point", "CREATING RESTORE POINT...")
              }
              icon={Shield}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#3b82f6"
            >
              CREATE RESTORE POINT
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("optimize-registry", "OPTIMIZING REGISTRY...")
              }
              icon={Settings}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#ec4899"
            >
              OPTIMIZE REGISTRY
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall(
                  "optimize-taskscheduler",
                  "OPTIMIZING SCHEDULER..."
                )
              }
              icon={Calendar}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#06b6d4"
            >
              OPTIMIZE TASK SCHEDULER
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("disable-updates", "DISABLING UPDATES...")
              }
              icon={DownloadCloud}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#22c55e"
            >
              DISABLE WINDOWS UPDATES
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("lower-visuals", "LOWERING VISUALS...")
              }
              icon={Eye}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#f59e0b"
            >
              LOWER VISUAL SETTINGS
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("set-darkmode", "SETTING DARK MODE...")
              }
              icon={Moon}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#6b7280"
            >
              SET DARK MODE
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("set-powerplan", "ADDING POWER PLAN...")
              }
              icon={Battery}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#a855f7"
            >
              ACTIVATE HST POWERPLAN
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall(
                  "remove-startup-apps",
                  "REMOVING STARTUP APPS..."
                )
              }
              icon={Zap}
              gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
              textColor="#ef4444"
            >
              REMOVE STARTUP APPS
            </NeonButton>
          </div>
        </div>

        {/* Bottom - 4 Cards */}
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(4, 1fr)",
            gap: "16px",
            marginBottom: "0px",
          }}
        >
          <OptionsPanel
            title="Services"
            options={serviceOptions}
            setOptions={setServiceOptions}
            onExecute={() => {
              const selected = Object.entries(serviceOptions).filter(
                ([_, checked]) => checked
              );
              if (selected.length === 0) {
                setCurrentStatus("NO SERVICES SELECTED");
                setTimeout(
                  () => setCurrentStatus("READY - SELECT AN OPERATION"),
                  2000
                );
              } else {
                const body = {
                  recommended: serviceOptions.recommended,
                  bluetooth: serviceOptions.bluetooth,
                  hyperv: serviceOptions.hyperv,
                  xbox: serviceOptions.xbox,
                };
                executeApiCall(
                  "optimize-services",
                  `OPTIMIZING SERVICES...`,
                  body
                );
              }
            }}
            color="#ffffff"
            gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
            icon={Activity}
          />

          <OptionsPanel
            title="Debloat"
            options={debloatOptions}
            setOptions={setDebloatOptions}
            onExecute={() => {
              const selected = Object.entries(debloatOptions).filter(
                ([_, checked]) => checked
              );
              if (selected.length === 0) {
                setCurrentStatus("NO ITEMS SELECTED");
                setTimeout(
                  () => setCurrentStatus("READY - SELECT AN OPERATION"),
                  2000
                );
              } else {
                const body = {
                  msApps: debloatOptions.msApps,
                  edge: debloatOptions.edge,
                  onedrive: debloatOptions.onedrive,
                  xboxApps: debloatOptions.xboxApps,
                  storeApps: debloatOptions.storeApps,
                };
                executeApiCall("debloat-apps", `REMOVING APPS...`, body);
              }
            }}
            color="#ffffff"
            gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
            icon={Trash2}
          />

          <OptionsPanel
            title="Cleanup"
            options={cleanupOptions}
            setOptions={setCleanupOptions}
            onExecute={() => {
              const selected = Object.entries(cleanupOptions).filter(
                ([_, checked]) => checked
              );
              if (selected.length === 0) {
                setCurrentStatus("NO CLEANUP ITEMS SELECTED");
                setTimeout(
                  () => setCurrentStatus("READY - SELECT AN OPERATION"),
                  2000
                );
              } else {
                const body = {
                  temp: cleanupOptions.temp,
                  cache: cleanupOptions.cache,
                  eventLog: cleanupOptions.eventLogs,
                  powerPlan: cleanupOptions.powerPlans,
                };
                executeApiCall("cleanup", `CLEANING...`, body);
              }
            }}
            color="#ffffff"
            gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
            icon={Sparkles}
          />

          <OptionsPanel
            title="Revert"
            options={revertOptions}
            setOptions={setRevertOptions}
            onExecute={() => {
              const selected = Object.entries(revertOptions).filter(
                ([_, checked]) => checked
              );
              if (selected.length === 0) {
                setCurrentStatus("NO OPTIONS SELECTED FOR REVERT");
                setTimeout(
                  () => setCurrentStatus("READY - SELECT AN OPERATION"),
                  2000
                );
              } else {
                const body = {
                  service: revertOptions.service,
                  task: revertOptions.task,
                  wUpdate: revertOptions.wUpdate,
                  registry: revertOptions.registry,
                };
                executeApiCall(
                  "revert-configurations",
                  `REVERTING CONFIGURATIONS...`,
                  body
                );
              }
            }}
            color="#ffffff"
            gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
            icon={RotateCcw}
          />
        </div>
      </div>
    </div>
  );
}