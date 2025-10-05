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

  // Checkbox states
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
    xboxapps: false,
    store: false,
  });

  const [cleanupOptions, setCleanupOptions] = useState({
    temp: false,
    cache: false,
    eventLogs: false,
    powerPlans: false,
  });

  useEffect(() => {
    checkApiStatus();
    fetchSystemInfo();
  }, []);

  const checkApiStatus = async () => {
    try {
      let response;
      try {
        response = await fetch("https://localhost:5001/api/system/test");
      } catch (httpsError) {
        response = await fetch("http://localhost:5000/api/system/test");
      }
      setApiStatus(response.ok ? "online" : "offline");
    } catch (error) {
      setApiStatus("offline");
    }
  };

  const fetchSystemInfo = async () => {
    try {
      let response;
      try {
        response = await fetch("https://localhost:5001/api/system/sysinfo");
      } catch (httpsError) {
        response = await fetch("http://localhost:5000/api/system/sysinfo");
      }

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

      let response;
      try {
        response = await fetch(
          `https://localhost:5001/api/system/${endpoint}`,
          options
        );
      } catch (httpsError) {
        response = await fetch(
          `http://localhost:5000/api/system/${endpoint}`,
          options
        );
      }

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
    disabled = false,
  }) => (
    <button
      onClick={onClick}
      disabled={disabled || activeOperation !== null}
      style={{
        width: "100%",
        height: "42px",
        background:
          gradient || "linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%)",
        border: "none",
        borderRadius: "8px",
        color: "#ffffff",
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
      <Icon style={{ width: "15px", height: "15px" }} />
      <span>{children}</span>
    </button>
  );

  const NeonCheckbox = ({ checked, onChange, label, color = "#3b82f6" }) => (
    <label
      style={{
        display: "flex",
        alignItems: "center",
        gap: "10px",
        cursor: "pointer",
        padding: "8px",
        borderRadius: "6px",
        transition: "all 0.2s ease",
        background: checked ? "rgba(59, 130, 246, 0.1)" : "transparent",
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.background = checked
          ? "rgba(59, 130, 246, 0.15)"
          : "rgba(255, 255, 255, 0.03)";
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.background = checked
          ? "rgba(59, 130, 246, 0.1)"
          : "transparent";
      }}
    >
      <div
        style={{
          width: "16px",
          height: "16px",
          borderRadius: "4px",
          border: `2px solid ${checked ? color : "#4b5563"}`,
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
              color: "white",
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
          color: "#e5e7eb",
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
      recommended: "Recommended",
      bluetooth: "Bluetooth",
      hyperv: "Hyper-V",
      xbox: "Xbox",
      msApps: "Microsoft Apps",
      edge: "Microsoft Edge",
      onedrive: "OneDrive",
      xboxapps: "Xbox Apps",
      store: "Microsoft Store",
      temp: "Temp Files",
      cache: "Cache",
      eventLogs: "Event Logs",
      powerPlans: "Power Plans",
    };

    return (
      <div
        style={{
          background: "#1f2937",
          border: "1px solid #374151",
          borderRadius: "12px",
          padding: "16px",
          boxShadow: "0 4px 12px rgba(0, 0, 0, 0.3)",
          transition: "all 0.2s ease",
          height: "100%",
          display: "flex",
          flexDirection: "column",
        }}
        onMouseEnter={(e) => {
          e.currentTarget.style.borderColor = "#4b5563";
          e.currentTarget.style.transform = "translateY(-1px)";
        }}
        onMouseLeave={(e) => {
          e.currentTarget.style.borderColor = "#374151";
          e.currentTarget.style.transform = "translateY(0)";
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "center",
            gap: "8px",
            marginBottom: "12px",
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

        <div style={{ marginBottom: "12px", flex: 1 }}>
          {Object.entries(options).map(([key, value]) => (
            <div key={key} style={{ marginBottom: "3px" }}>
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

        <NeonButton onClick={onExecute} gradient={gradient} icon={Icon}>
          Execute
        </NeonButton>
      </div>
    );
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #0f172a 0%, #1e293b 100%)",
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
          linear-gradient(rgba(59, 130, 246, 0.03) 1px, transparent 1px),
          linear-gradient(90deg, rgba(59, 130, 246, 0.03) 1px, transparent 1px)
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
          background: "#1f2937",
          border: "1px solid #374151",
          borderRadius: "50%",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          cursor: "pointer",
          transition: "all 0.2s ease",
          boxShadow: "0 2px 8px rgba(0, 0, 0, 0.3)",
        }}
        onMouseEnter={(e) => {
          e.currentTarget.style.background = "#374151";
          e.currentTarget.style.borderColor = "#4b5563";
        }}
        onMouseLeave={(e) => {
          e.currentTarget.style.background = "#1f2937";
          e.currentTarget.style.borderColor = "#374151";
        }}
      >
        <X style={{ width: "18px", height: "18px", color: "#9ca3af" }} />
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
          <div
            style={{
              display: "inline-flex",
              alignItems: "center",
              gap: "16px",
              marginBottom: "16px",
            }}
          >
            <img
              src="/hst-high-resolution-logo-transparent.png"
              alt="HST"
              style={{ width: "26px", height: "26px" }}
            />
            <h1
              style={{
                fontSize: "42px",
                fontWeight: "900",
                color: "#f3f4f6",
                letterSpacing: "2px",
                margin: 0,
                textShadow: "0 2px 10px rgba(0, 0, 0, 0.5)",
              }}
            >
              HST
            </h1>
          </div>
          <p
            style={{
              color: "#e5e7eb",
              fontSize: "14px",
              fontWeight: "700",
              marginBottom: "4px",
              letterSpacing: "1px",
              textTransform: "uppercase",
            }}
          >
            Windows Optimization Utility
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
                  ? "rgba(34, 197, 94, 0.1)"
                  : "rgba(239, 68, 68, 0.1)",
              borderRadius: "16px",
              border: `1px solid ${
                apiStatus === "online" ? "#22c55e" : "#ef4444"
              }`,
              marginTop: "16px",
            }}
          >
            {apiStatus === "online" ? (
              <Wifi
                style={{ width: "14px", height: "14px", color: "#22c55e" }}
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
                color: apiStatus === "online" ? "#22c55e" : "#ef4444",
                textTransform: "uppercase",
              }}
            >
              API {apiStatus}
            </span>
          </div>

          {/* Status Bar */}
          <div
            style={{
              background: "#1f2937",
              border: "1px solid #374151",
              borderRadius: "8px",
              padding: "12px 16px",
              marginTop: "16px",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.3)",
            }}
          >
            <div
              style={{
                display: "flex",
                alignItems: "center",
                gap: "8px",
              }}
            >
              {activeOperation ? (
                <Loader
                  style={{ width: "16px", height: "16px", color: "#3b82f6" }}
                  className="animate-spin"
                />
              ) : (
                <CheckCircle
                  style={{ width: "16px", height: "16px", color: "#22c55e" }}
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
            gridTemplateColumns: "1fr 280px",
            gap: "16px",
            marginBottom: "16px",
          }}
        >
          {/* Left - System Info Panel */}
          <div
            style={{
              background: "#1f2937",
              border: "1px solid #374151",
              borderRadius: "12px",
              padding: "18px",
              boxShadow: "0 4px 12px rgba(0, 0, 0, 0.3)",
            }}
          >
            <div
              style={{
                display: "flex",
                alignItems: "center",
                gap: "8px",
                marginBottom: "16px",
              }}
            >
              <Server
                style={{ width: "18px", height: "18px", color: "#3b82f6" }}
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
              }}
            >
              {[
                {
                  label: "USER",
                  value: systemInfo.user,
                  icon: User,
                  color: "#22c55e",
                },
                {
                  label: "TIME",
                  value: systemInfo.time,
                  icon: Watch,
                  color: "#22c55e",
                },
                {
                  label: "GPU",
                  value: systemInfo.gpu,
                  icon: Monitor,
                  color: "#3b82f6",
                },
                {
                  label: "CPU",
                  value: systemInfo.cpu,
                  icon: Cpu,
                  color: "#ef4444",
                },
                {
                  label: "RAM",
                  value: systemInfo.ram,
                  icon: MemoryStick,
                  color: "#f59e0b",
                },
                {
                  label: "Storage",
                  value: systemInfo.storage,
                  icon: HardDrive,
                  color: "#22c55e",
                },
              ].map((item, i) => (
                <div
                  key={i}
                  style={{
                    background: "#111827",
                    border: `1px solid #374151`,
                    borderRadius: "8px",
                    padding: "12px",
                    transition: "all 0.2s ease",
                    cursor: "pointer",
                  }}
                  onMouseEnter={(e) => {
                    e.currentTarget.style.borderColor = item.color;
                    e.currentTarget.style.background = "#1f2937";
                  }}
                  onMouseLeave={(e) => {
                    e.currentTarget.style.borderColor = "#374151";
                    e.currentTarget.style.background = "#111827";
                  }}
                >
                  <div
                    style={{
                      display: "flex",
                      alignItems: "center",
                      gap: "6px",
                      marginBottom: "6px",
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
                  background: "#111827",
                  border: "1px solid #374151",
                  borderRadius: "6px",
                  color: "green",
                  textDecoration: "none",
                  fontWeight: "700",
                  fontSize: "11px",
                  letterSpacing: "0.5px",
                  transition: "all 0.2s ease",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.borderColor = "#4b5563";
                  e.currentTarget.style.color = "#e5e7eb";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.borderColor = "#374151";
                  e.currentTarget.style.color = "green";
                }}
              >
                <Github style={{ width: "14px", height: "14px" }} />
                GITHUB
              </a>
            </div>
          </div>

          {/* Right - 7 Operation Buttons */}
          <div
            style={{ display: "grid", gridTemplateColumns: "1fr", gap: "7px" }}
          >
            <NeonButton
              onClick={() =>
                executeApiCall("restore-point", "CREATING RESTORE POINT...")
              }
              icon={Shield}
              gradient="linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%)"
            >
              Create Restore Point
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("optimize-registry", "OPTIMIZING REGISTRY...")
              }
              icon={Settings}
              gradient="linear-gradient(135deg, #ec4899 0%, #be185d 100%)"
            >
              Optimize Registry
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall(
                  "optimize-taskscheduler",
                  "OPTIMIZING SCHEDULER..."
                )
              }
              icon={Calendar}
              gradient="linear-gradient(135deg, #06b6d4 0%, #0891b2 100%)"
            >
              Task Scheduler
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("disable-updates", "DISABLING UPDATES...")
              }
              icon={DownloadCloud}
              gradient="linear-gradient(135deg, #22c55e 0%, #16a34a 100%)"
            >
              Disable Updates
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("lower-visuals", "LOWERING VISUALS...")
              }
              icon={Eye}
              gradient="linear-gradient(135deg, #f59e0b 0%, #d97706 100%)"
            >
              Lower Visuals
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("set-darkmode", "SETTING DARK MODE...")
              }
              icon={Moon}
              gradient="linear-gradient(135deg, #6b7280 0%, #4b5563 100%)"
            >
              Dark Mode
            </NeonButton>
            <NeonButton
              onClick={() =>
                executeApiCall("set-powerplan", "ADDING POWER PLAN...")
              }
              icon={Battery}
              gradient="linear-gradient(135deg, #a855f7 0%, #7e22ce 100%)"
            >
              Power Plan
            </NeonButton>
          </div>
        </div>

        {/* Bottom - Three Option Panels */}
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(3, 1fr)",
            gap: "16px",
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
            color="#22c55e"
            gradient="linear-gradient(135deg, #22c55e 0%, #16a34a 100%)"
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
                  xboxapps: debloatOptions.xboxapps,
                  store: debloatOptions.store,
                };
                executeApiCall("debloat-apps", `REMOVING APPS...`, body);
              }
            }}
            color="#ef4444"
            gradient="linear-gradient(135deg, #ef4444 0%, #dc2626 100%)"
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
                  eventLogs: cleanupOptions.eventLogs,
                  powerPlans: cleanupOptions.powerPlans,
                };
                executeApiCall("cleanup", `CLEANING...`, body);
              }
            }}
            color="#a855f7"
            gradient="linear-gradient(135deg, #a855f7 0%, #7e22ce 100%)"
            icon={Sparkles}
          />
        </div>
      </div>
    </div>
  );
}
