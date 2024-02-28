import React, { useState, useEffect, useCallback } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const fetchDataFromEndpoint = async () => {
  const response = await fetch('/weather');
  if (!response.ok) {
    throw new Error('Network response is invalid');
  }
  return await response.json();
};

const Graphs = () => {
  const [data, setData] = useState([]);
  const [showLatestOnly, setShowLatestOnly] = useState(false);
  const [timer, setTimer] = useState(60);

  const loadData = useCallback(async () => {
    try {
      const fetchedData = await fetchDataFromEndpoint();
      setData(fetchedData);
      setTimer(60);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    }
  }, []);

  useEffect(() => {
    loadData(); 
    const intervalId = setInterval(loadData, 60000); 
    return () => clearInterval(intervalId); 
  }, [loadData]);

  useEffect(() => {
    const timerId = timer > 0 && setInterval(() => setTimer(timer - 1), 1000);
    return () => clearInterval(timerId); 
  }, [timer]);

  const toggleDataView = () => {
    setShowLatestOnly(!showLatestOnly);
  };

  const displayedData = showLatestOnly ? data.map((series) => ({ 
    ...series,
    data: [series.data[series.data.length - 1]], 
  })) : data;

    return (
        <div id="dev" style={{ padding: '20px' }}>
            <button onClick={toggleDataView} style={{ marginBottom: '20px' }}>
                {showLatestOnly ? 'Show Data From Last 2 Hours' : 'Show Latest Data Only'} 
            </button>

            <div style={{ marginBottom: '40px' }}>
                <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Min Temperature Graph</h2>
                <ResponsiveContainer width="100%" height={300}>
                    <LineChart data={displayedData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="time" type="category" allowDuplicatedCategory={false} />
                        <YAxis dataKey="temperature" />
                        <Tooltip />
                        <Legend />
                        {displayedData.map((s) => (
                            <Line type="monotone" dataKey="temperature" data={s.data} name={s.name} key={s.name} />
                        ))}
                    </LineChart>
                </ResponsiveContainer>
            </div>

            <div style={{ marginBottom: '40px' }}>
                <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Wind Speed Graph</h2>
                <ResponsiveContainer width="100%" height={300}>
                    <LineChart data={displayedData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="time" type="category" allowDuplicatedCategory={false} />
                        <YAxis dataKey="windSpeed" />
                        <Tooltip />
                        <Legend />
                        {displayedData.map((s) => (
                            <Line type="monotone" dataKey="windSpeed" data={s.data} name={s.name} key={s.name} />
                        ))}
                    </LineChart>
                </ResponsiveContainer>
            </div>

            <p>Next data refresh in: {timer} seconds</p>
        </div>
    );

};

export default Graphs;
