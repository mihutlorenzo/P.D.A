import java.util.concurrent.locks.ReentrantLock;

public class Buffer {
	private final int BUFFERLENGTH = 10;
	private int[] buffer;
	private int bufferTail, bufferHead, numberOfElementInBuffer;
	private Object condProd;
	private Object condCons;
	private ReentrantLock lock;

	public Buffer() {
		buffer = new int[BUFFERLENGTH];
		bufferTail = 0;
		bufferHead = 0;
		numberOfElementInBuffer = 0;
		lock = new ReentrantLock();
		condProd = new Object();
		condCons = new Object();
	}

	public int get() {
		int value = 0;
		try {

			lock.lock();
			if (numberOfElementInBuffer <= 0) {
				condCons.wait();
			}
			value = buffer[bufferHead % buffer.length];
			numberOfElementInBuffer--;
			bufferHead++;
			System.out.println("Consumer get element from buffer with value " + value + " and number in queue "
					+ bufferHead % buffer.length);
			lock.unlock();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			condProd.notify();
		}
		return value;
	}

	public void put(int value) {
		try {

			lock.lock();
			if (numberOfElementInBuffer >= BUFFERLENGTH) {
					condProd.wait();
			}
			buffer[bufferTail % buffer.length] = value;
			numberOfElementInBuffer++;
			bufferTail++;
			System.out.println("Producer put in buffer element with value " + value + " and number in queue "
					+ bufferTail % buffer.length);
			lock.unlock();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			condCons.notify();

		}
	}

}
