import java.util.concurrent.Semaphore;
import java.util.concurrent.locks.ReentrantLock;

public class Consumer implements Runnable {
	private Buffer buffer;
	private int id;

	public Consumer(Buffer buffer, int id) {
		super();
		this.buffer = buffer;
		this.id = id;
		
	}

	@Override
	public void run() {
		while (true) {
			int value = buffer.get();
				
			try {
				Thread.sleep(300);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

}