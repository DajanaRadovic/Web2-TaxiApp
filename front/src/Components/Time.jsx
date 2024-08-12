   /* import React, {useState, useEffect} from 'react';

    const Time = ({dolazak, krajnje, kraj})=>{
        const [preostaloVremeDolaska, setPreostaloVremeDolaska] = useState(dolazak * 60);
        const [preostaloVremeZavrsetka, setPreostaloVremeZavrsetka] = useState(krajnje * 60);
        const [pocetakVoznje, setPocetakVoznje] = useState(false);

        useEffect(() => {
            const dolazakTimer = setInterval(() => {
                setPreostaloVremeDolaska(prev => {
                    if (prev > 0) {
                        return prev - 1;
                    } else {
                        clearInterval(dolazakTimer);
                        setPocetakVoznje(true);
                        return 0;
                    }
                });
            }, 1000);
    
            return () => {
                clearInterval(dolazakTimer);
            };
        }, []);


        useEffect(() => {
            if (pocetakVoznje) {
                const tripEndTimer = setInterval(() => {
                    setPreostaloVremeZavrsetka(prev => (prev > 0 ? prev - 1 : 0));
                }, 1000);
    
                return () => {
                    clearInterval(tripEndTimer);
                };
            }
        }, [pocetakVoznje]);
    
        useEffect(() => {
            if (preostaloVremeZavrsetka === 0) {
                kraj(); // Call API or perform any action when trip ends
            }
        }, [preostaloVremeZavrsetka, kraj]);
    
        const formatTime = seconds => {
            const mins = Math.floor(seconds / 60);
            const secs = seconds % 60;
            return `${mins}:${secs < 10 ? '0' : ''}${secs}`;
        };

        return (
            <div style={{ color: 'white', marginTop: '30px' }}>
                {!pocetakVoznje ? (
                    <div>Time to arrive: {formatTime(preostaloVremeDolaska)}</div>
                ) : (
                    <div>
                        <div>Ride is started</div>
                        <div>Time to end ride: {formatTime(preostaloVremeZavrsetka)}</div>
                    </div>
                )}
            </div>
        );
    }   
    export default Time;*/