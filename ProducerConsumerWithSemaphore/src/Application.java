

public class Application {
	
	static final int PRODUCERNUMBER = 7;
	static final int CONSUMERNUMBER = 2;

	static Buffer buffer = new Buffer();
	
	static Thread[] producers = new Thread[PRODUCERNUMBER];
	static Thread[] consumers = new Thread[CONSUMERNUMBER];

	public static void main(String[] args) {
		
		for(int i = 0;i<PRODUCERNUMBER ;i++) {
			producers[i] = new Thread(new Producer(buffer,i));
		}
		
		for(int i = 0;i<CONSUMERNUMBER;i++) {
			consumers[i] = new Thread(new Consumer(buffer,i));
		}
		
		for(int i = 0;i<PRODUCERNUMBER ;i++) {
			producers[i].start();
		}
		
		for(int i = 0;i<CONSUMERNUMBER;i++) {
			consumers[i].start();
		}
		
		for(int i = 0;i<PRODUCERNUMBER ;i++) {
			try {
				producers[i].join();
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		
		for(int i = 0;i<CONSUMERNUMBER;i++) {
			try {
				consumers[i].join();
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

	}

}
