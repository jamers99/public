window.networkHelper = {
    isOnline: () => navigator.onLine,
    registerOnlineOfflineHandlers: (dotNetRef) => {
        window.addEventListener('online', () => dotNetRef.invokeMethodAsync('SetOnlineStatus', true));
        window.addEventListener('offline', () => dotNetRef.invokeMethodAsync('SetOnlineStatus', false));
    }
};