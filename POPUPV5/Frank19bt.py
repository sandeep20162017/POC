import warnings
from pandas.errors import SettingWithCopyWarning
import requests
import pandas as pd
import ta
import numpy as np
from datetime import datetime, timedelta, time, date
import time as ttime
import random
import logging
import json
from scipy.stats import linregress
from scipy.signal import argrelextrema
import warnings
import pytz
import argparse
import sys
from collections import defaultdict
from ta.trend import ADXIndicator

pd.set_option('future.no_silent_downcasting', True)
warnings.filterwarnings('ignore', category=SettingWithCopyWarning)

logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(message)s',
    handlers=[logging.StreamHandler()]
)
warnings.filterwarnings('ignore', category=FutureWarning, message="Series.fillna with 'method' is deprecated")
warnings.filterwarnings('ignore', category=RuntimeWarning)

# ===== USER-CONFIGURABLE VARIABLES =====

TICKERS =  ['METC']


START_DATE = datetime(2023, 11, 24)
END_DATE = datetime(2024, 8, 8)
MIN_WEEKLY_DATA = 52
MIN_DAILY_DATA = 200
PROFIT_TARGET_CALL = 0.17
STOP_LOSS_CALL = -0.05
PROFIT_TARGET_PUT = 0.17
STOP_LOSS_PUT = -0.05
MAX_HOLD_DAYS = 15
INITIAL_CAPITAL = 10000
MAX_ACTIVE_TRADES = 5
MARKET_OPEN = time(9, 30)
MARKET_CLOSE = time(16, 0)
TIMEZONE = pytz.timezone('America/New_York')

def is_market_open():
    now = datetime.now(TIMEZONE)
    current_time = now.time()
    current_day = now.weekday()
    if current_day >= 5:
        return False
    return MARKET_OPEN <= current_time <= MARKET_CLOSE

def rate_limit(min_delay=10, max_delay=20):
    ttime.sleep(random.uniform(min_delay, max_delay))

def get_current_price_and_volume(ticker):
    url = f"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}"
    params = {'interval': '2m', 'range': '1d'}
    headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'}
    
    try:
        rate_limit()
        logging.info(f"Fetching real-time data for {ticker} from Yahoo Finance")
        response = requests.get(url, params=params, headers=headers, timeout=15)
        response.raise_for_status()
        data = response.json()
        
        if 'chart' not in data or 'result' not in data['chart'] or not data['chart']['result']:
            logging.warning("Could not find real-time data in Yahoo response")
            return 0, 0, "Failed"
            
        result = data['chart']['result'][0]
        meta = result['meta']
        current_price = meta.get('regularMarketPrice', meta.get('chartPreviousClose', 0))
        
        if 'indicators' in result and 'quote' in result['indicators']:
            quotes = result['indicators']['quote'][0]
            if 'volume' in quotes and len(quotes['volume']) > 0:
                current_volume = sum([v for v in quotes['volume'] if v is not None])
                return float(current_price), float(current_volume), "RealTime"
        
        return float(current_price), 0, "PriceOnly"
            
    except Exception as e:
        logging.error(f"Yahoo Finance real-time fetch failed: {str(e)}")
        return 0, 0, "Failed"
    
def get_historical_data(ticker, interval='1d', period='52w', start_date=None, end_date=None):
    if start_date and end_date:
        period1 = int(start_date.timestamp())
        period2 = int(end_date.timestamp())
        daily_df = get_yahoo_data(ticker, '1d', period1=period1, period2=period2)
    else:
        daily_df = get_yahoo_data(ticker, '1d', period=period)
    
    if daily_df.empty or len(daily_df) < 20:
        logging.error(f"Insufficient data for {ticker}")
        return pd.DataFrame()
    
    if interval == '1wk':
        weekly_df = daily_df.resample('W-FRI').agg({
            'Open': 'first',
            'High': 'max',
            'Low': 'min',
            'Close': 'last',
            'Volume': 'sum'
        }).dropna()
        return weekly_df
    else:
        return daily_df
    
def get_yahoo_data(ticker, interval, period=None, period1=None, period2=None):
    range_map = {'52w': '1y', '2y': '2y', 'max': 'max'}
    interval_map = {'1d': '1d', '1wk': '1wk'}
    
    url = f"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}"
    params = {'interval': interval_map.get(interval, '1d')}
    
    if period:
        params['range'] = range_map.get(period, '2y')
    elif period1 and period2:
        params['period1'] = period1
        params['period2'] = period2
    
    headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'}
    
    try:
        rate_limit()
        logging.info(f"Fetching {interval} data for {ticker} from Yahoo Finance API")
        response = requests.get(url, params=params, headers=headers, timeout=15)
        response.raise_for_status()
        data = response.json()
        
        if 'chart' not in data or 'result' not in data['chart'] or not data['chart']['result']:
            logging.error("No chart data found in response")
            return pd.DataFrame()
            
        result = data['chart']['result'][0]
        timestamps = result['timestamp']
        quotes = result['indicators']['quote'][0]
        
        df = pd.DataFrame({
            'Date': pd.to_datetime(timestamps, unit='s'),
            'Open': quotes['open'],
            'High': quotes['high'],
            'Low': quotes['low'],
            'Close': quotes['close'],
            'Volume': quotes['volume']
        })
        df.set_index('Date', inplace=True)
        df.sort_index(ascending=True, inplace=True)
        df = df.dropna()
        
        logging.info(f"Retrieved {len(df)} rows from Yahoo Finance")
        return df
        
    except Exception as e:
        logging.error(f"Yahoo Finance API failed for {ticker}: {str(e)}")
        return pd.DataFrame()

def ichimoku(df):
    if df.empty or len(df) < 52:
        return (
            pd.Series(dtype=float), 
            pd.Series(dtype=float), 
            pd.Series(dtype=float), 
            pd.Series(dtype=float), 
            pd.Series(dtype=float)
        )
    high = df['High']
    low = df['Low']
    close = df['Close']
    
    window_9 = min(9, len(df))
    window_26 = min(26, len(df))
    window_52 = min(52, len(df))
    
    conversion = (high.rolling(window_9).max() + low.rolling(window_9).min()) / 2
    base = (high.rolling(window_26).max() + low.rolling(window_26).min()) / 2
    span_a = ((conversion + base) / 2).shift(window_26)
    span_b = (high.rolling(window_52).max() + low.rolling(window_52).min()) / 2
    span_b = span_b.shift(window_26)
    lagging_span = close.shift(-window_26)
    
    span_a = span_a.ffill()
    span_b = span_b.ffill()
    
    return conversion, base, span_a, span_b, lagging_span

def detect_candle_patterns(df):
    try:
        body = np.abs(df['Close'] - df['Open'])
        full_range = df['High'] - df['Low']
        safe_range = np.where(full_range == 0, 1e-6, full_range)
        lower_shadow = np.minimum(df['Close'], df['Open']) - df['Low']
        upper_shadow = df['High'] - np.maximum(df['Close'], df['Open'])
        
        hammer = (body < full_range * 0.3) & (lower_shadow > body * 2) & (upper_shadow < body * 0.5)
        shooting_star = (body < full_range * 0.3) & (upper_shadow > body * 2) & (lower_shadow < body * 0.5)
        doji = (body < safe_range * 0.1) & (full_range > 0)
        
        prev_close = df['Close'].shift(1).fillna(df['Close'])
        prev_open = df['Open'].shift(1).fillna(df['Open'])
        
        bullish_engulfing = (df['Close'] > df['Open']) & (prev_close < prev_open) & \
                            (df['Open'] < prev_close) & (df['Close'] > prev_open)
        bearish_engulfing = (df['Close'] < df['Open']) & (prev_close > prev_open) & \
                             (df['Open'] > prev_close) & (df['Close'] < prev_open)
        
        return hammer, shooting_star, doji, bullish_engulfing, bearish_engulfing
        
    except Exception as e:
        logging.error(f"Candle pattern error: {str(e)}")
        false_array = np.full(len(df), False)
        return false_array, false_array, false_array, false_array, false_array

def detect_complex_patterns(df, pattern_type, timeframe):
    patterns = []
    if len(df) < 20:
        return patterns
    
    close = df['Close'].values
    high = df['High'].values
    low = df['Low'].values
    
    if pattern_type == 'head_shoulders' and timeframe == 'weekly':
        try:
            max_idx = argrelextrema(high, np.greater, order=5)[0]
            if len(max_idx) < 3:
                return patterns
            peaks = sorted(max_idx, key=lambda i: high[i], reverse=True)[:3]
            peaks.sort()
            
            if len(peaks) == 3:
                left_shoulder, head, right_shoulder = peaks
                neckline = min(df['Low'].iloc[left_shoulder], df['Low'].iloc[right_shoulder])
                if (high[left_shoulder] < high[head] > high[right_shoulder] and
                    abs(high[left_shoulder] - high[right_shoulder]) < high[head] * 0.05 and
                    df['Close'].iloc[-1] < neckline):
                    patterns.append(('HEAD_SHOULDERS', -40, "Major reversal pattern"))
        except Exception as e:
            logging.error(f"Head & Shoulders detection failed: {e}")
    
    elif pattern_type == 'cup_handle':
        try:
            lookback = 120 if timeframe == 'daily' else 40
            if len(df) < lookback:
                return patterns
            min_idx = argrelextrema(low, np.less, order=5)[0]
            if len(min_idx) < 1:
                return patterns
            cup_low_idx = min_idx[-1]
            cup_start_idx = max(0, cup_low_idx - lookback//2)
            cup_end_idx = min(len(df)-1, cup_low_idx + lookback//2)
            cup_high_left = high[cup_start_idx]
            cup_high_right = high[cup_end_idx]
            cup_depth = (high[cup_start_idx] - low[cup_low_idx]) / high[cup_start_idx]
            
            if (abs(cup_high_left - cup_high_right) < cup_high_left * 0.08 and
                cup_depth > 0.15 and
                cup_end_idx > cup_low_idx and cup_end_idx < len(df)-5):
                handle_start = cup_end_idx
                handle_df = df.iloc[handle_start:]
                if len(handle_df) > 5:
                    handle_min = handle_df['Low'].min()
                    handle_max = handle_df['High'].max()
                    handle_depth = (handle_max - handle_min) / handle_max
                    if handle_depth < 0.08 and df['Close'].iloc[-1] > handle_max:
                        patterns.append(('CUP_HANDLE', 35, "Bullish continuation pattern"))
        except Exception as e:
            logging.error(f"Cup & Handle detection failed: {e}")
    
    elif pattern_type == 'flag_pennant' and timeframe == 'daily':
        try:
            if len(df) < 15:
                return patterns
            max_move = (df['High'].rolling(5).max() - df['Low'].rolling(5).min()).max()
            recent_move = (df['High'].iloc[-1] - df['Low'].iloc[-1]) / df['Close'].iloc[-1]
            if max_move > 0.1 and recent_move < 0.03:
                patterns.append(('FLAG_PENNANT', 20, "Continuation pattern forming"))
        except Exception as e:
            logging.error(f"Flag/Pennant detection failed: {e}")
    
    return patterns

def detect_rsi_divergence(df, timeframe):
    divergences = []
    if len(df) < 30:
        return divergences
    
    try:
        rsi = ta.momentum.rsi(df['Close'], window=14)
        price_peaks = argrelextrema(df['Close'].values, np.greater, order=5)[0]
        rsi_peaks = argrelextrema(rsi.values, np.greater, order=5)[0]
        price_troughs = argrelextrema(df['Close'].values, np.less, order=5)[0]
        rsi_troughs = argrelextrema(rsi.values, np.less, order=5)[0]
        
        if len(price_troughs) > 1 and len(rsi_troughs) > 1:
            last_price_trough = price_troughs[-1]
            prev_price_trough = price_troughs[-2]
            last_rsi_trough = rsi_troughs[-1]
            prev_rsi_trough = rsi_troughs[-2]
            if (df['Close'].iloc[last_price_trough] < df['Close'].iloc[prev_price_trough] and
                rsi.iloc[last_rsi_trough] > rsi.iloc[prev_rsi_trough] and
                last_price_trough > prev_price_trough):
                strength = 25 if timeframe == 'weekly' else 15
                divergences.append(('BULL_RSI_DIV', strength, "Bullish momentum divergence"))
        
        if len(price_peaks) > 1 and len(rsi_peaks) > 1:
            last_price_peak = price_peaks[-1]
            prev_price_peak = price_peaks[-2]
            last_rsi_peak = rsi_peaks[-1]
            prev_rsi_peak = rsi_peaks[-2]
            if (df['Close'].iloc[last_price_peak] > df['Close'].iloc[prev_price_peak] and
                rsi.iloc[last_rsi_peak] < rsi.iloc[prev_rsi_peak] and
                last_price_peak > prev_price_peak):
                strength = -25 if timeframe == 'weekly' else -15
                divergences.append(('BEAR_RSI_DIV', strength, "Bearish momentum divergence"))
    except Exception as e:
        logging.error(f"RSI Divergence detection failed: {e}")
    
    return divergences

def weekly_macro_score(weekly_df):
    if weekly_df.empty or len(weekly_df) < MIN_WEEKLY_DATA:
        return 0, "❌ Insufficient weekly data"
    
    conversion, base, span_a, span_b, lagging_span = ichimoku(weekly_df)
    current_close = weekly_df['Close'].iloc[-1]
    current_open = weekly_df['Open'].iloc[-1]
    cloud_bullish = current_close > max(span_a.iloc[-1], span_b.iloc[-1])
    cloud_bearish = current_close < min(span_a.iloc[-1], span_b.iloc[-1])
    cloud_thickness = abs(span_a.iloc[-1] - span_b.iloc[-1])
    cloud_trend = 1 if span_a.iloc[-1] > span_b.iloc[-1] else -1
    
    adx_val = 0
    try:
        adx_val = ta.trend.adx(weekly_df['High'], weekly_df['Low'], weekly_df['Close'], window=14).iloc[-1]
    except Exception as e:
        logging.warning(f"ADX calculation failed: {e}")
    
    rsi_val = 0
    try:
        rsi_val = ta.momentum.rsi(weekly_df['Close'], window=14).iloc[-1]
    except Exception as e:
        logging.warning(f"RSI calculation failed: {e}")
    
    volume_trend_score = 0
    volume_notes = []
    try:
        if len(weekly_df) >= 4:
            volumes = weekly_df['Volume'].tail(4)
            slope, _, _, _, _ = linregress(range(4), volumes)
            if slope > 0:
                volume_trend_score = 10
                volume_notes.append("↑ Volume Trend")
            elif slope < 0:
                volume_trend_score = -5
                volume_notes.append("↓ Volume Trend")
    except Exception as e:
        logging.warning(f"Volume trend analysis failed: {e}")
    
    volume_spike = False
    try:
        avg_volume = weekly_df['Volume'].rolling(10).mean().iloc[-1]
        current_volume = weekly_df['Volume'].iloc[-1]
        volume_ratio = current_volume / avg_volume if avg_volume > 0 else 1
        volume_spike = volume_ratio > 1.8
        volume_notes.append(f"Vol: {volume_ratio:.1f}x")
    except Exception as e:
        logging.warning(f"Volume spike detection failed: {e}")
    
    score = 0
    notes = []
    
    if cloud_bullish:
        score += 30
        notes.append("✅ Cloud Bullish")
        if cloud_trend > 0:
            score += 10
            notes.append("Strong Cloud Structure")
    elif cloud_bearish:
        score -= 30
        notes.append("❌ Cloud Bearish")
        if cloud_trend < 0:
            score -= 10
            notes.append("Strong Cloud Structure")
    else:
        score -= 10
        notes.append("⚠️ Price In Cloud")
    
    if adx_val > 25:
        score += 25
        notes.append(f"✅ ADX {adx_val:.1f}")
    elif adx_val < 15:
        score -= 15
        notes.append(f"⚠️ Weak Trend (ADX {adx_val:.1f})")
    
    if rsi_val < 35:
        score += 15
        notes.append(f"✅ Oversold (RSI {rsi_val:.1f})")
    elif rsi_val > 70:
        score -= 15
        notes.append(f"⚠️ Overbought (RSI {rsi_val:.1f})")
    elif 40 < rsi_val < 60:
        score += 5
        notes.append(f"↔️ Neutral RSI {rsi_val:.1f}")
        
    if volume_spike:
        if current_close > current_open:
            score += 10
            notes.append("✅ Bull Vol Spike")
        elif current_close < current_open:
            score -= 10
            notes.append("❌ Bear Vol Spike")
    else:
        score += volume_trend_score
        notes.extend(volume_notes)
    
    if cloud_thickness > 0:
        if cloud_thickness > weekly_df['Close'].mean() * 0.03:
            if cloud_bullish:
                score += 5
                notes.append("Thick Support Cloud")
            elif cloud_bearish:
                score -= 5
                notes.append("Thick Resistance Cloud")
    
    patterns = []
    if not weekly_df.empty and len(weekly_df) > 40:
        patterns.extend(detect_complex_patterns(weekly_df, 'head_shoulders', 'weekly'))
        patterns.extend(detect_complex_patterns(weekly_df, 'cup_handle', 'weekly'))
        patterns.extend(detect_rsi_divergence(weekly_df, 'weekly'))
    
    pattern_score = 0
    pattern_notes = []
    for pattern, score_val, note in patterns:
        pattern_score += score_val
        pattern_notes.append(note)
    
    score += pattern_score
    if pattern_notes:
        notes.extend(pattern_notes)
    
    return score, " + ".join(notes) if notes else "❌ No strong signals"

def detect_ttm_squeeze(df):
    try:
        bb = ta.volatility.BollingerBands(close=df['Close'], window=20, window_dev=2)
        df['BB_Upper'] = bb.bollinger_hband()
        df['BB_Lower'] = bb.bollinger_lband()
        df['BB_Mid'] = bb.bollinger_mavg()
        df['BB_Width'] = (df['BB_Upper'] - df['BB_Lower']) / df['BB_Mid']
        
        df['ATR'] = ta.volatility.average_true_range(df['High'], df['Low'], df['Close'], window=20)
        df['KC_Mid'] = df['Close'].rolling(20).mean()
        df['KC_Upper'] = df['KC_Mid'] + 1.5 * df['ATR']
        df['KC_Lower'] = df['KC_Mid'] - 1.5 * df['ATR']
        df['KC_Width'] = (df['KC_Upper'] - df['KC_Lower']) / df['KC_Mid']
        
        df['Squeeze_On'] = df['BB_Width'] < df['KC_Width']
        df['Squeeze_Off'] = ~df['Squeeze_On']
        
        df['Bull_Breakout'] = (df['Close'] > df['BB_Upper']) & (df['Close'] > df['KC_Upper'])
        df['Bear_Breakout'] = (df['Close'] < df['BB_Lower']) & (df['Close'] < df['KC_Lower'])
        
        df['MACD_Hist'] = ta.trend.macd_diff(df['Close'])
        df['MACD_Rising'] = df['MACD_Hist'] > df['MACD_Hist'].shift(1)
        df['RSI'] = ta.momentum.rsi(df['Close'], window=14)
        
        df['Valid_Bull_Squeeze'] = df['Bull_Breakout'] & df['MACD_Rising'] & (df['RSI'] < 70)
        df['Valid_Bear_Squeeze'] = df['Bear_Breakout'] & ~df['MACD_Rising'] & (df['RSI'] > 30)
        
        # Add consecutive squeeze days calculation
        df['Squeeze_Consecutive'] = 0
        if not df.empty:
            s = df['Squeeze_On']
            groups = (s != s.shift()).cumsum()
            df['Squeeze_Consecutive'] = df.groupby(groups).cumcount() + 1
            df.loc[~s, 'Squeeze_Consecutive'] = 0

        return df
    except Exception as e:
        logging.error(f"TTM Squeeze detection failed: {str(e)}")
        return df

def daily_signal_score(df, weekly_df, current_volume=0, backtest_mode=False):
    if df.empty:
        return 0, 0, "⚠️ Empty DataFrame", "⚠️ Empty DataFrame", None
    if len(df) < MIN_DAILY_DATA:
        return 0, 0, f"⚠️ Insufficient data ({len(df)}<{MIN_DAILY_DATA})", f"⚠️ Insufficient data ({len(df)}<{MIN_DAILY_DATA})", None
    
    try:
        if len(df) >= 15:
            adx_indicator = ta.trend.ADXIndicator(
                df['High'], df['Low'], df['Close'], window=14)
            adx_val = adx_indicator.adx().iloc[-1]
        else:
            adx_val = 0
            
        if len(df) >= 15:
            atr_val = ta.volatility.average_true_range(
                df['High'], df['Low'], df['Close'], window=14).iloc[-1]
            atr_pct = (atr_val / df['Close'].iloc[-1]) * 100
        else:
            atr_pct = 0
    except Exception as e:
        logging.error(f"Indicator calculation error: {str(e)}")
        adx_val = 0
        atr_pct = 0
    
    df = detect_ttm_squeeze(df)
    # Add debug information for squeeze detection
    if not df.empty and 'Squeeze_On' in df.columns:
        last_row = df.iloc[-1]
        logging.debug(
            f"Squeeze Debug - BB Width: {last_row.get('BB_Width', 0):.6f}, "
            f"KC Width: {last_row.get('KC_Width', 0):.6f}, "
            f"Squeeze On: {last_row.get('Squeeze_On', False)}, "
            f"Consecutive Days: {last_row.get('Squeeze_Consecutive', 0)}"
            )
    
    weekly_trend_up = False
    weekly_trend_down = False
    try:
        if isinstance(weekly_df, pd.DataFrame) and len(weekly_df) >= 4:
            weekly_trend_up = weekly_df['Close'].iloc[-1] > weekly_df['Close'].iloc[-4]
            weekly_trend_down = weekly_df['Close'].iloc[-1] < weekly_df['Close'].iloc[-4]
    except Exception as e:
        logging.warning(f"Weekly trend error: {str(e)}")
    
    try:
        conversion, base, span_a, span_b, lagging_span = ichimoku(df)
        df['Above_Cloud'] = (df['Close'] > span_a) & (df['Close'] > span_b)
        df['Below_Cloud'] = (df['Close'] < span_a) & (df['Close'] < span_b)
        df['Cloud_Trend'] = np.where(span_a > span_b, 1, -1)
        
        df['Volume_MA20'] = df['Volume'].rolling(20).mean()
        last_index = df.index[-1]
        
        if backtest_mode or not is_market_open():
            current_volume = df['Volume'].iloc[-1]
        df.at[last_index, 'Volume'] = current_volume
        df.at[last_index, 'Volume_vs_Avg'] = max(0, (current_volume / df['Volume_MA20'].iloc[-1]) * 100)
        
        try:
            hammer, ss, doji, bull_eng, bear_eng = detect_candle_patterns(df)
            df['Hammer'] = hammer
            df['Shooting_Star'] = ss
            df['Doji'] = doji
            df['Bullish_Engulfing'] = bull_eng
            df['Bearish_Engulfing'] = bear_eng
        except Exception as e:
            logging.error(f"Candle pattern error: {str(e)}")
            for col in ['Hammer', 'Shooting_Star', 'Doji', 'Bullish_Engulfing', 'Bearish_Engulfing']:
                df[col] = False

        last, prev = df.iloc[-1], df.iloc[-2]
        
        bullish_score = 0
        bearish_score = 0
        bull_notes = []
        bear_notes = []
        
        # Enhanced volatility scoring
        if atr_pct > 2.5:
            volatility_score = 15
            bull_notes.append(f"✅ High Volatility (ATR:{atr_pct:.2f}%)")
            bear_notes.append(f"✅ High Volatility (ATR:{atr_pct:.2f}%)")
        elif atr_pct > 1.8:
            volatility_score = 10
            bull_notes.append(f"↑ Medium Volatility (ATR:{atr_pct:.2f}%)")
            bear_notes.append(f"↑ Medium Volatility (ATR:{atr_pct:.2f}%)")
        else:
            volatility_score = -10
            bull_notes.append(f"⚠️ Low Volatility (ATR:{atr_pct:.2f}%)")
            bear_notes.append(f"⚠️ Low Volatility (ATR:{atr_pct:.2f}%)")
        
        bullish_score += volatility_score
        bearish_score += volatility_score
        
        # Enhanced trend scoring
        if adx_val > 25:
            trend_score = 20
            bull_notes.append(f"✅ Strong Trend (ADX:{adx_val:.1f})")
            bear_notes.append(f"✅ Strong Trend (ADX:{adx_val:.1f})")
        elif adx_val > 20:
            trend_score = 10
            bull_notes.append(f"↑ Medium Trend (ADX:{adx_val:.1f})")
            bear_notes.append(f"↑ Medium Trend (ADX:{adx_val:.1f})")
        elif adx_val < 15:
            trend_score = -15
            bull_notes.append(f"⚠️ Weak Trend (ADX:{adx_val:.1f})")
            bear_notes.append(f"⚠️ Weak Trend (ADX:{adx_val:.1f})")
        else:
            trend_score = 0
        
        bullish_score += trend_score
        bearish_score += trend_score
        
        # Squeeze-based scoring
        if last.get('Valid_Bull_Squeeze', False):
            bullish_score += 75
            vol_boost = min(35, max(0, (last['Volume_vs_Avg'] - 100) / 2))
            bullish_score += vol_boost
            bull_notes.append(f"TTM BULL SQUEEZE (Vol: {last['Volume_vs_Avg']:.0f}%)")
            if weekly_trend_up:
                bullish_score += 25
                bull_notes.append("Weekly Trend Boost")
            elif not weekly_trend_down:
                bullish_score += 15
                
        elif last.get('Valid_Bear_Squeeze', False):
            bearish_score += 75
            vol_boost = min(35, max(0, (last['Volume_vs_Avg'] - 100) / 2))
            bearish_score += vol_boost
            bear_notes.append(f"TTM BEAR SQUEEZE (Vol: {last['Volume_vs_Avg']:.0f}%)")
            if weekly_trend_down:
                bearish_score += 25
                bear_notes.append("Weekly Trend Boost")
            elif not weekly_trend_up:
                bearish_score += 15
                
        else:
            if last.get('Above_Cloud', False):
                bullish_score += 25
                bull_notes.append("Above Cloud")
                if last.get('Cloud_Trend', 0) > 0:
                    bullish_score += 15
                    bull_notes.append("Bullish Cloud")
                    
            if last.get('Hammer', False):
                bullish_score += 20
                bull_notes.append("Hammer")
                
            if last.get('MACD_Rising', False):
                bullish_score += 15
                bull_notes.append("MACD Rising")
                
            if last.get('Below_Cloud', False):
                bearish_score += 25
                bear_notes.append("Below Cloud")
                if last.get('Cloud_Trend', 0) < 0:
                    bearish_score += 15
                    bear_notes.append("Bearish Cloud")
                    
            if last.get('Shooting_Star', False):
                bearish_score += 20
                bear_notes.append("Shooting Star")
                
            if not last.get('MACD_Rising', False):
                bearish_score += 15
                bear_notes.append("MACD Falling")
        
        if atr_pct > 2.0:
            bullish_score *= 1.2
            bearish_score *= 1.2
        elif atr_pct < 1.0:
            bullish_score *= 0.8
            bearish_score *= 0.8
        
        if bullish_score > 50 and bearish_score > 50:
            neutral_factor = 0.7
            bullish_score *= neutral_factor
            bearish_score *= neutral_factor
            bull_notes.append("⚠️ Strong Conflict")
            bear_notes.append("⚠️ Strong Conflict")
        elif bullish_score > 70 and bearish_score > 30:
            bearish_score *= 0.6
            bear_notes.append("⚠️ Bull Dominance")
        elif bearish_score > 70 and bullish_score > 30:
            bullish_score *= 0.6
            bull_notes.append("⚠️ Bear Dominance")
        
        last['ADX'] = adx_val
        last['ATR%'] = atr_pct
        
        if last is not None:
            last['Squeeze_Status'] = "On" if last.get('Squeeze_On', False) else "Off"
            last['Squeeze_Days'] = int(last.get('Squeeze_Consecutive', 0))
            last['Squeeze_Direction'] = (
                "Bullish" if last.get('Valid_Bull_Squeeze', False) else
                "Bearish" if last.get('Valid_Bear_Squeeze', False) else "None"
            )
        return bullish_score, bearish_score, " | ".join(bull_notes), " | ".join(bear_notes), last
        
    except Exception as e:
        logging.error(f"DAILY SIGNAL ERROR: {str(e)}", exc_info=True)
        return 0, 0, f"⚠️ Indicator error: {str(e)}", "N/A", None

def calculate_support_resistance(df):
    if df.empty or len(df) < 20:
        return 0, 0
    support = df['Low'].tail(20).min()
    resistance = df['High'].tail(20).max()
    if len(df) > 20:
        bb = ta.volatility.BollingerBands(close=df['Close'], window=20, window_dev=2)
        current_close = df['Close'].iloc[-1]
        if abs(current_close - resistance) < (resistance - support) * 0.1:
            resistance = bb.bollinger_hband().iloc[-1]
        if abs(current_close - support) < (resistance - support) * 0.1:
            support = bb.bollinger_lband().iloc[-1]
    return support, resistance

def interpret_volume(vol_change, vol_vs_avg, price_action, market_status):
    if not price_action:
        return "Volume data unavailable"
    price_up = price_action['Close'] > price_action['Open']
    price_down = price_action['Close'] < price_action['Open']
    price_flat = abs(price_action['Close'] - price_action['Open']) < (price_action['High'] - price_action['Low']) * 0.1
    strength = []
    warning = []
    conclusion = []
    if market_status == "AfterHours":
        conclusion.append("After-Hours: Use caution with volume data")
    elif market_status == "PreMarket":
        conclusion.append("Pre-Market: Volume incomplete")
    if vol_vs_avg > 150:
        strength.append("exceptional volume")
    elif vol_vs_avg > 120:
        strength.append("strong volume")
    elif vol_vs_avg > 90:
        strength.append("average volume")
    else:
        warning.append("below-average volume")
    if vol_change > 50:
        strength.append("sharp increase from yesterday")
    elif vol_change > 20:
        strength.append("significant increase from yesterday")
    elif vol_change < -20:
        warning.append("notable decrease from yesterday")
    if price_up:
        if vol_vs_avg > 120:
            conclusion.append("BULLISH VOLUME CONFIRMATION")
        elif vol_vs_avg < 80:
            conclusion.append("warning: rally on weak volume")
    elif price_down:
        if vol_vs_avg > 120:
            conclusion.append("BEARISH VOLUME CONFIRMATION")
        elif vol_vs_avg < 80:
            conclusion.append("warning: decline on weak volume")
    elif price_flat:
        if vol_vs_avg > 120:
            conclusion.append("accumulation/distribution possible")
    if vol_vs_avg > 150 and vol_change > 50:
        conclusion.append("institutional activity detected")
    elif vol_vs_avg < 70 and vol_change < -30:
        conclusion.append("potential lack of conviction")
    interpretation = ""
    if strength:
        interpretation += "Strength: " + ", ".join(strength) + ". "
    if warning:
        interpretation += "Caution: " + ", ".join(warning) + ". "
    if conclusion:
        interpretation += "Interpretation: " + ", ".join(conclusion) + "."
    return interpretation if interpretation else "Volume: Neutral - no significant signals"

def highlight_patterns(notes):
    if not notes or notes == "None":
        return "None"
    patterns = {
        "HEAD_SHOULDERS": "🔴 HEAD & SHOULDERS",
        "CUP_HANDLE": "🟢 CUP & HANDLE",
        "FLAG_PENNANT": "🟩 FLAG/PENNANT",
        "RSI_DIV": "↕️ RSI DIVERGENCE",
        "BULL_RSI_DIV": "↗️ BULL RSI DIV",
        "BEAR_RSI_DIV": "↘️ BEAR RSI DIV"
    }
    for pattern, display in patterns.items():
        notes = notes.replace(pattern, display)
    return notes

def generate_detailed_report(ticker, entry_rec, base_score, ref, price, price_source,
                            fifty_two_week_low, fifty_two_week_high, conflict_analysis,
                            entry_timing, timing_details, price_action, market_status):
    if not ref or not price_action:
        return f"{ticker} ➜ SKIPPED | Insufficient data for analysis"
    vol_change = ref.get('Volume_Change', 0)
    vol_vs_avg = ref.get('Volume_vs_Avg', 100)
    adx_val = ref.get('ADX', 0)
    atr_pct = ref.get('ATR%', 0)
    
    if vol_change >= 0:
        vol_daily_str = f"+{vol_change:.0f}% from yesterday"
    else:
        vol_daily_str = f"{vol_change:.0f}% from yesterday"
    
    volume_interpretation = interpret_volume(vol_change, vol_vs_avg, price_action, market_status)
    
    entry_emoji = entry_rec.split()[0] if entry_rec else "◻️"
    entry_text = " ".join(entry_rec.split()[1:]) if entry_rec else "NO SIGNAL"
    
    report = (
        f"\n{entry_emoji} {ticker} ➜ {entry_text} | Conviction: {base_score:.0f}/100\n"
        f"Price: ${price:.2f} | Trend: {ref.get('Trend', 'N/A')} | Vol: {vol_daily_str}\n"
        f"ADX: {adx_val:.1f} (Trend Strength) | ATR%: {atr_pct:.2f}% (Volatility)\n"
        f"Support: ${ref.get('Support', 0):.2f} | Resistance: ${ref.get('Resistance', 0):.2f} | "
        f"52w Range: ${fifty_two_week_low:.2f}-${fifty_two_week_high:.2f}\n"
        f"Market Status: {market_status}\n"
        f"Key Patterns:\n"
        f"- WEEKLY: {highlight_patterns(ref.get('MacroNote', 'None'))}\n"
        f"- DAILY: Bull: {highlight_patterns(ref.get('DailyBullNote', 'None'))} | "
        f"Bear: {highlight_patterns(ref.get('DailyBearNote', 'None'))}\n"
        f"Volume Analysis: {volume_interpretation}\n"
        f"Conflict Resolution: {conflict_analysis}\n"
        f"Entry Timing: {entry_timing}\n"
        f"  → {timing_details}"
    )
    report += (
        f"Squeeze Status: {ref.get('Squeeze_Status', 'N/A')} | "
        f"# Days: {ref.get('Squeeze_Days', 0)} | "
        f"Direction: {ref.get('Squeeze_Direction', 'N/A')}\n"
    )
    return report

def run_recommendation():
    results = []
    market_open = is_market_open()
    market_status = "Open" if market_open else "Closed"
    
    for ticker in TICKERS:
        logging.info(f"Processing {ticker}")
        daily_df = get_historical_data(ticker, '1d', '52w')
        if daily_df.empty:
            logging.warning(f"{ticker} ➜ No daily data. Skipping.")
            continue

        weekly_df = daily_df.resample('W-FRI').agg({
            'Open': 'first',
            'High': 'max',
            'Low': 'min',
            'Close': 'last',
            'Volume': 'sum'
        }).dropna()
        
        if len(weekly_df) >= MIN_WEEKLY_DATA:
            weekly_df = weekly_df.iloc[-MIN_WEEKLY_DATA:]
        else:
            logging.warning(f"Only {len(weekly_df)} weeks of data for {ticker}")
        
        current_price, current_volume, source = get_current_price_and_volume(ticker)
        if current_price == 0:
            if not daily_df.empty:
                current_price = daily_df['Close'].iloc[-1]
                source = "DailyClose"
            elif not weekly_df.empty:
                current_price = weekly_df['Close'].iloc[-1]
                source = "WeeklyClose"
            else:
                current_price = 0
                source = "Failed"
        
        if not daily_df.empty and len(daily_df) > 200:
            fifty_two_week_low = daily_df['Low'].min()
            fifty_two_week_high = daily_df['High'].max()
        elif not weekly_df.empty and len(weekly_df) > 40:
            fifty_two_week_low = weekly_df['Low'].min()
            fifty_two_week_high = weekly_df['High'].max()
        else:
            fifty_two_week_low = 0
            fifty_two_week_high = 0

        macro_score, macro_note = weekly_macro_score(weekly_df)
        daily_bull, daily_bear, daily_bull_note, daily_bear_note, last = daily_signal_score(daily_df, weekly_df, current_volume)
        
        if last is None:
            base_score = 0
            entry_rec = "SKIP"
            conflict_analysis = "Insufficient data"
            trend_label = "N/A"
            support, resistance = 0, 0
            entry_timing = "N/A"
            timing_details = "N/A"
            volume_change = 0
            volume_vs_avg = 100
            price_action = {'Open': 0, 'Close': 0, 'High': 0, 'Low': 0}
            adx_val = 0
            atr_pct = 0
        else:
            price_action = {
                'Open': last['Open'],
                'Close': last['Close'],
                'High': last['High'],
                'Low': last['Low']
            }
            net_daily_score = daily_bull - daily_bear
            conflict_analysis = f"Bull: {daily_bull_note} | Bear: {daily_bear_note} | Macro: {macro_note}"
            adx_val = last.get('ADX', 0)
            atr_pct = last.get('ATR%', 0)
            base_score = (macro_score + net_daily_score + 170) * 100 / 340
            base_score = max(0, min(100, base_score))
            if adx_val > 20:
                base_score = min(100, base_score + 10)
            if atr_pct < 1.0:
                base_score = max(0, base_score - 15)
            adx_val = last.get('ADX', 0)
            trend_strength = "strong" if adx_val > 30 else "moderate" if adx_val > 20 else "weak"
            trend_label = f"{trend_strength.capitalize()} Trend (ADX:{adx_val:.1f})"
            if net_daily_score >= 85:
                entry_rec = "🟢 STRONG CALL ENTRY"
            elif net_daily_score >= 65:
                entry_rec = "🟡 CALL SETUP"
            elif net_daily_score >= 50:
                entry_rec = "⚪ WATCH_CALLS"
            elif net_daily_score <= -85:
                entry_rec = "🔴 STRONG PUT ENTRY"
            elif net_daily_score <= -65:
                entry_rec = "🟣 PUT SETUP"
            elif net_daily_score <= -50:
                entry_rec = "⚫ WATCH_PUTS"
            elif net_daily_score >= 30:
                entry_rec = "🟠 MILD_CALL_BIAS"
            elif net_daily_score <= -30:
                entry_rec = "🟤 MILD_PUT_BIAS"
            else:
                entry_rec = "◻️ NEUTRAL"
            support, resistance = calculate_support_resistance(daily_df)
            current_price = last['Close']
            mid_point = (support + resistance) / 2
            upper_third = resistance - (resistance - support) / 3
            lower_third = support + (resistance - support) / 3
            vol_ratio = last.get('Volume_vs_Avg', 100)
            if net_daily_score > 40:
                if current_price > upper_third:
                    if vol_ratio > 150:
                        entry_timing = "ENTER NOW - Breakout with volume"
                        timing_details = "Price breaking resistance with strong volume confirmation "
                    else:
                        entry_timing = "Wait for pullback to support"
                        timing_details = f"Look for entry near ${support:.2f} with volume increase "
                elif current_price > mid_point:
                    if vol_ratio > 120:
                        entry_timing = "ENTER NOW - Momentum building"
                        timing_details = "Price in upper range with increasing volume"
                    else:
                        entry_timing = "Enter on dip"
                        timing_details = f"Buy near ${mid_point:.2f} with stop below ${support:.2f}"
                else:
                    entry_timing = "Accumulate at support"
                    timing_details = f"Buy near ${support:.2f} with volume confirmation "
            elif net_daily_score < -40:
                if current_price < lower_third:
                    if vol_ratio > 150:
                        entry_timing = "ENTER NOW - Breakdown with volume"
                        timing_details = "Price breaking support with strong volume confirmation"
                    else:
                        entry_timing = "Wait for bounce to resistance"
                        timing_details = f"Look for entry near ${resistance:.2f} with volume increase"
                elif current_price < mid_point:
                    if vol_ratio > 120:
                        entry_timing = "ENTER NOW - Momentum building"
                        timing_details = "Price in lower range with increasing volume"
                    else:
                        entry_timing = "Enter on bounce"
                        timing_details = f"Sell near ${mid_point:.2f} with stop above ${resistance:.2f}"
                else:
                    entry_timing = "Short at resistance"
                    timing_details = f"Sell near ${resistance:.2f} with volume confirmation"
            else:
                entry_timing = "Wait for confirmation"
                timing_details = "Monitor key levels for volume-backed move"
            volume_change = last.get('Volume_Pct_Change', 0)
            volume_vs_avg = last.get('Volume_vs_Avg', 100)
        
        ref = {
            "MacroNote": macro_note,
            "DailyBullNote": daily_bull_note,
            "DailyBearNote": daily_bear_note,
            "Support": support,
            "Resistance": resistance,
            "Trend": trend_label,
            "Volume_Change": volume_change,
            "Volume_vs_Avg": volume_vs_avg,
            "ADX": adx_val,
            "ATR%": atr_pct,
            "Squeeze_Status": last.get('Squeeze_Status', 'N/A'),
            "Squeeze_Days": last.get('Squeeze_Days', 0),
            "Squeeze_Direction": last.get('Squeeze_Direction', 'N/A')
        }
        
        report_line = generate_detailed_report(
            ticker, entry_rec, base_score, ref, current_price, source,
            fifty_two_week_low, fifty_two_week_high, conflict_analysis,
            entry_timing, timing_details, price_action, market_status
        )
        
        results.append({"Symbol": ticker, "Report": report_line})

    if results:
        print('\n' + '=' * 120)
        print("OPTIONS TRADING SIGNAL REPORT".center(120))
        print('=' * 120)
        for result in results:
            print(result["Report"])
            print('-' * 120)
        print('=' * 120)
    else:
        print("\n⚠️ No qualified trades today.")

def run_backtest():
    logging.basicConfig(
        level=logging.DEBUG,
        format='%(asctime)s - %(levelname)s - %(message)s',
        handlers=[
            logging.StreamHandler(),
            logging.FileHandler('backtest_debug.log', mode='w')
        ]
    )
    logger = logging.getLogger()
    logger.info("===== BACKTEST STARTED =====")
    
    capital = INITIAL_CAPITAL
    active_trades = []
    closed_trades = []
    trade_id = 1
    active_tickers = set()
    max_profit = None
    max_loss = None
    total_profit = 0
    signals_log = []
    signal_count = 0
    days_with_signals = 0
    missing_data_dates = defaultdict(list)
    
    logger.info(f"Starting backtest for {TICKERS} from {START_DATE} to {END_DATE}")
    logger.info(f"Strategy parameters: CALL PT={PROFIT_TARGET_CALL*100:.0f}%, "
                f"PUT PT={PROFIT_TARGET_PUT*100:.0f}%, "
                f"Max trades={MAX_ACTIVE_TRADES}, Max hold days={MAX_HOLD_DAYS}")

    data = {}
    for ticker in TICKERS:
        logger.info(f"Fetching data for {ticker}")
        try:
            start_date_with_buffer = START_DATE - timedelta(days=400)
            logger.debug(f"Requesting data from {start_date_with_buffer} to {END_DATE}")
            df = get_historical_data(
                ticker, 
                '1d', 
                start_date=start_date_with_buffer,
                end_date=END_DATE
            )
            if df.empty:
                logger.warning(f"No data for {ticker}. Skipping.")
                continue
            if df.index[0] > START_DATE:
                logger.error(f"CRITICAL: Data for {ticker} starts after backtest period")
                logger.error(f"Backtest starts: {START_DATE}, First data point: {df.index[0]}")
                continue
            if df.index[-1] < END_DATE:
                logger.warning(f"Data for {ticker} ends before backtest period")
            data[ticker] = df
            logger.info(f"Data for {ticker}: {len(df)} rows from {df.index[0].date()} to {df.index[-1].date()}")
        except Exception as e:
            logger.error(f"Error loading data for {ticker}: {str(e)}")
    
    all_trading_days = set()
    for df in data.values():
        mask = (df.index >= START_DATE) & (df.index <= END_DATE)
        all_trading_days.update(df.index[mask])
    trading_days_sorted = sorted(all_trading_days)
    logger.info(f"Backtesting {len(trading_days_sorted)} trading days")

    for current_date in trading_days_sorted:
        current_date_str = current_date.strftime('%Y-%m-%d')
        logger.debug(f"\n=== PROCESSING DATE: {current_date_str} ===")    
         
        trades_to_close = []
        for trade in active_trades:
            ticker = trade['ticker']
            if ticker not in data or current_date not in data[ticker].index:
                continue
            current_price = data[ticker].loc[current_date, 'Close']
            hold_days = (current_date - trade['entry_date']).days
            position_value = 400
            shares = position_value / trade['entry_price']
            
            if len(data[ticker]) >= 15:
                atr_indicator = ta.volatility.AverageTrueRange(
                    data[ticker]['High'], 
                    data[ticker]['Low'], 
                    data[ticker]['Close'], 
                    window=14
                )
                current_atr = atr_indicator.average_true_range().loc[current_date]
            else:
                current_atr = 0
            
            exit_reason = None
            if trade['type'] == 'CALL':
                if current_atr > 0:
                    trailing_stop = current_price - 1.5 * current_atr
                    if current_price < trailing_stop:
                        exit_reason = 'Trailing Stop'
                if not exit_reason and current_price >= trade['entry_price'] * (1 + PROFIT_TARGET_CALL):
                    exit_reason = 'Profit Target'
                if not exit_reason and current_price <= trade['entry_price'] * (1 + STOP_LOSS_CALL):
                    exit_reason = 'Stop Loss'
            elif trade['type'] == 'PUT':
                if current_atr > 0:
                    trailing_stop = current_price + 1.5 * current_atr
                    if current_price > trailing_stop:
                        exit_reason = 'Trailing Stop'
                if not exit_reason and current_price <= trade['entry_price'] * (1 - PROFIT_TARGET_PUT):
                    exit_reason = 'Profit Target'
                if not exit_reason and current_price >= trade['entry_price'] * (1 - STOP_LOSS_PUT):
                    exit_reason = 'Stop Loss'
            if not exit_reason and hold_days >= MAX_HOLD_DAYS:
                exit_reason = 'Max Hold Days'
            
            if exit_reason:
                if trade['type'] == 'CALL':
                    trade['profit'] = (current_price - trade['entry_price']) * shares
                else:
                    trade['profit'] = (trade['entry_price'] - current_price) * shares
                trade['profit_pct'] = (trade['profit'] / position_value) * 100
                trade['exit_date'] = current_date
                trade['exit_price'] = current_price
                trade['exit_reason'] = exit_reason
                trade['hold_days'] = hold_days
                if max_profit is None or trade['profit'] > max_profit:
                    max_profit = trade['profit']
                if max_loss is None or trade['profit'] < max_loss:
                    max_loss = trade['profit']
                total_profit += trade['profit']
                trades_to_close.append(trade)
                active_tickers.remove(ticker)
                logger.info(f"Closing trade {trade['id']} for {ticker}: {exit_reason} "
                            f"(P/L: ${trade['profit']:.2f}, {trade['profit_pct']:.2f}%)")
        
        for trade in trades_to_close:
            active_trades.remove(trade)
            closed_trades.append(trade)
        
        if len(active_trades) >= MAX_ACTIVE_TRADES:
            logger.debug(f"Max active trades reached ({MAX_ACTIVE_TRADES}), skipping new entries")
            continue
            
        for ticker in TICKERS:
            if ticker not in data:
                continue
            if current_date not in data[ticker].index:
                missing_data_dates[ticker].append(current_date_str)
                logger.debug(f"Skipping {ticker} - no data for {current_date_str}")
                continue
            if ticker in active_tickers:
                logger.debug(f"{ticker} already in active trades")
                continue
            logger.debug(f"Evaluating {ticker} for {current_date_str}")
            daily_slice = data[ticker].loc[:current_date]
            if len(daily_slice) < MIN_DAILY_DATA:
                logger.warning(f"{ticker} on {current_date_str}: Insufficient daily data "
                               f"({len(daily_slice)} < {MIN_DAILY_DATA})")
                continue
            weekly_slice = daily_slice.resample('W-FRI').agg({
                'Open': 'first',
                'High': 'max',
                'Low': 'min',
                'Close': 'last',
                'Volume': 'sum'
            }).dropna()
            if len(weekly_slice) < MIN_WEEKLY_DATA:
                logger.warning(f"{ticker} on {current_date_str}: Insufficient weekly data "
                               f"({len(weekly_slice)} < {MIN_WEEKLY_DATA})")
                continue
            current_price = daily_slice.iloc[-1]['Close']
            current_volume = daily_slice.iloc[-1]['Volume']
            logger.debug(f"{ticker} price: ${current_price:.2f}, volume: {current_volume}")
            
            try:
                macro_score, macro_note = weekly_macro_score(weekly_slice)
                logger.debug(f"Weekly macro score: {macro_score}, notes: {macro_note}")
                daily_bull, daily_bear, daily_bull_note, daily_bear_note, last = daily_signal_score(
                    daily_slice, weekly_slice, current_volume, backtest_mode=True)
                if last is None:
                    logger.warning(f"Signal generation failed for {ticker} on {current_date_str}")
                    continue
                net_daily_score = daily_bull - daily_bear
                conflict_analysis = f"Bull: {daily_bull_note} | Bear: {daily_bear_note} | Macro: {macro_note}"
                adx_val = last.get('ADX', 0)
                atr_pct = last.get('ATR%', 0)
                volume_ratio = current_volume / daily_slice['Volume'].rolling(20).mean().iloc[-1]
                volume_ok = volume_ratio > 1.25
                trade_type = None
                trade_reason = ""
                if "TTM BULL SQUEEZE" in daily_bull_note and adx_val > 22 and atr_pct > 1.8 and volume_ok:
                    trade_type = "CALL"
                    trade_reason = "Valid Bull Squeeze"
                elif "TTM BEAR SQUEEZE" in daily_bear_note and adx_val > 22 and atr_pct > 1.8 and volume_ok:
                    trade_type = "PUT"
                    trade_reason = "Valid Bear Squeeze"
                elif net_daily_score >= 75 and adx_val > 22 and atr_pct > 1.8 and volume_ok:
                    trade_type = "CALL"
                    trade_reason = "Strong Bull Signal"
                elif net_daily_score <= -75 and adx_val > 22 and atr_pct > 1.8 and volume_ok:
                    trade_type = "PUT"
                    trade_reason = "Strong Bear Signal"
                signal_data = {
                    'date': current_date,
                    'ticker': ticker,
                    'signal_type': trade_reason if trade_type else "No Trade",
                    'net_score': net_daily_score,
                    'bull_notes': daily_bull_note,
                    'bear_notes': daily_bear_note,
                    'macro_note': macro_note,
                    'price': current_price,
                    'adx': adx_val,
                    'atr_pct': atr_pct
                }
                signals_log.append(signal_data)
                signal_count += 1
                if trade_type:
                    days_with_signals += 1
                logger.info(f"{current_date_str} {ticker}: {trade_reason if trade_type else 'No Trade'} "
                            f"(Score: {net_daily_score:.1f}, ADX: {adx_val:.1f}, ATR%: {atr_pct:.2f})")
                logger.debug(f"Bull notes: {daily_bull_note}")
                logger.debug(f"Bear notes: {daily_bear_note}")
                logger.debug(f"Conflict analysis: {conflict_analysis}")
                if trade_type:
                    risk_adjustment = min(2.0, max(0.5, atr_pct / 2.0))
                    position_value = 400 * risk_adjustment
                    shares = position_value / current_price
                    trade = {
                        'id': trade_id,
                        'ticker': ticker,
                        'type': trade_type,
                        'entry_date': current_date,
                        'entry_price': current_price,
                        'shares': shares,
                        'position_value': position_value,
                        'exit_date': None,
                        'exit_price': None,
                        'exit_reason': None,
                        'profit': None,
                        'profit_pct': None,
                        'hold_days': None,
                        'signal': trade_reason,
                        'reason': trade_reason,
                        'details': conflict_analysis
                    }
                    active_trades.append(trade)
                    active_tickers.add(ticker)
                    trade_id += 1
                    logger.info(f"Opened {trade_type} trade for {ticker} at ${current_price:.2f} - {trade_reason}")
                else:
                    logger.debug(f"No trade opened: Score {net_daily_score:.1f}, ADX {adx_val:.1f}, ATR% {atr_pct:.2f}")
            except Exception as e:
                logger.error(f"Error processing {ticker} on {current_date_str}: {str(e)}", exc_info=True)
    
    logger.info("Closing remaining trades at end of backtest")
    for trade in active_trades:
        ticker = trade['ticker']
        if ticker in data and not data[ticker].empty:
            last_date = data[ticker].index[-1]
            current_price = data[ticker].loc[last_date, 'Close']
            hold_days = (last_date - trade['entry_date']).days
            if trade['type'] == 'CALL':
                trade['profit'] = (current_price - trade['entry_price']) * trade['shares']
            else:
                trade['profit'] = (trade['entry_price'] - current_price) * trade['shares']
            trade['profit_pct'] = (trade['profit'] / trade['position_value']) * 100
            trade['exit_date'] = last_date
            trade['exit_price'] = current_price
            trade['exit_reason'] = 'End of backtest'
            trade['hold_days'] = hold_days
            if max_profit is None or trade['profit'] > max_profit:
                max_profit = trade['profit']
            if max_loss is None or trade['profit'] < max_loss:
                max_loss = trade['profit']
            total_profit += trade['profit']
            closed_trades.append(trade)
            logger.info(f"Closed trade {trade['id']} for {ticker} at end of backtest "
                         f"(P/L: ${trade['profit']:.2f}, {trade['profit_pct']:.2f}%)")
    
    winning_trades = [t for t in closed_trades if t['profit'] > 0]
    win_rate = (len(winning_trades) / len(closed_trades)) * 100 if closed_trades else 0
    
    print('\n' + '=' * 120)
    print("BACKTEST REPORT".center(120))
    print(f"Period: {START_DATE.strftime('%Y-%m-%d')} to {END_DATE.strftime('%Y-%m-%d')}")
    print(f"Initial Capital: ${INITIAL_CAPITAL:,.2f}")
    print(f"Total Profit/Loss: ${total_profit:,.2f} ({total_profit/INITIAL_CAPITAL*100:.2f}%)")
    print(f"Win Rate: {win_rate:.2f}%")
    max_profit_display = f"${max_profit:,.2f}" if max_profit is not None else "N/A"
    max_loss_display = f"${max_loss:,.2f}" if max_loss is not None else "N/A"
    print(f"Max Profit: {max_profit_display} | Max Loss: {max_loss_display}")
    print('=' * 120)
    
    if closed_trades:
        print(f"{'ID':<4} {'Ticker':<6} {'Type':<6} {'Entry Date':<12} {'Entry Price':>12} {'Exit Date':<12} "
              f"{'Exit Price':>12} {'Profit/Loss':>12} {'P/L %':>8} {'Days':>5} {'Exit Reason':<20} {'Signal'}")
        print('-' * 120)
        for trade in closed_trades:
            pl_color = '\033[92m' if trade['profit'] >= 0 else '\033[91m'
            reset_color = '\033[0m'
            print(f"{trade['id']:<4} {trade['ticker']:<6} {trade['type']:<6} "
                  f"{trade['entry_date'].strftime('%Y-%m-%d'):<12} ${trade['entry_price']:>11.2f} "
                  f"{trade['exit_date'].strftime('%Y-%m-%d'):<12} ${trade['exit_price']:>11.2f} "
                  f"{pl_color}${trade['profit']:>11.2f}{reset_color} "
                  f"{pl_color}{trade['profit_pct']:>7.2f}%{reset_color} "
                  f"{trade['hold_days']:>5} "
                  f"{trade['exit_reason']:<20} "
                  f"{trade['signal']}")
    else:
        print("NO TRADES EXECUTED DURING BACKTEST PERIOD")
    print('=' * 120)
    
    final_capital = INITIAL_CAPITAL + total_profit
    roi = (final_capital - INITIAL_CAPITAL) / INITIAL_CAPITAL * 100
    print(f"\nFINAL CAPITAL: ${final_capital:,.2f} | ROI: {roi:.2f}%")
    print(f"Total Trades: {len(closed_trades)} | Winning Trades: {len(winning_trades)} | Win Rate: {win_rate:.2f}%")
    if max_profit is not None and max_loss is not None:
        print(f"Max Profit: ${max_profit:,.2f} | Max Loss: ${max_loss:,.2f}")
    else:
        print("Max Profit: N/A | Max Loss: N/A")
    
    print("\n" + "="*120)
    print("SIGNAL ANALYSIS REPORT".center(120))
    print("="*120)
    print(f"Total trading days: {len(trading_days_sorted)}")
    print(f"Days with signals generated: {days_with_signals}")
    print(f"Total signals processed: {signal_count}")
    print(f"Signals with score > 0: {len([s for s in signals_log if s['net_score'] > 0])}")
    print(f"Signals with score < 0: {len([s for s in signals_log if s['net_score'] < 0])}")
    print(f"Signals with score = 0: {len([s for s in signals_log if s['net_score'] == 0])}")
    
    if signals_log:
        print("\nTop 10 signals by absolute score:")
        sorted_signals = sorted(signals_log, key=lambda x: abs(x['net_score']), reverse=True)[:10]
        for i, sig in enumerate(sorted_signals):
            print(f"{i+1}. {sig['date'].date()} {sig['ticker']}:")
            print(f"   Signal Type: {sig.get('signal_type', 'N/A')}")
            print(f"   Score: {sig['net_score']:.1f}")
            print(f"   Price: ${sig['price']:.2f}")
            print(f"   ADX: {sig.get('adx', 0):.1f}, ATR%: {sig.get('atr_pct', 0):.2f}")
            print(f"   Bull Notes: {sig['bull_notes']}")
            print(f"   Bear Notes: {sig['bear_notes']}")
            print(f"   Macro Notes: {sig['macro_note']}")
            print("-" * 80)
        print("\nMost frequent signals:")
        signal_counts = {}
        for sig in signals_log:
            signal_type = sig.get('signal_type', 'UNKNOWN')
            signal_counts[signal_type] = signal_counts.get(signal_type, 0) + 1
        for signal, count in sorted(signal_counts.items(), key=lambda x: x[1], reverse=True)[:10]:
            print(f"{signal}: {count} times")
    else:
        print("\nNo signals generated during backtest period")
    
    if missing_data_dates:
        print("\n" + "="*120)
        print("MISSING DATA REPORT".center(120))
        print("="*120)
        for ticker, dates in missing_data_dates.items():
            print(f"{ticker}: Missing {len(dates)} trading days")
            if dates:
                print(f"First missing date: {dates[0]}")
                print(f"Last missing date: {dates[-1]}")
            print("-"*80)
    else:
        print("\nNo missing data during backtest period")
    
    print("\nDEBUG NOTE: Detailed logs saved to 'backtest_debug.log'")
    print("="*120)
    
    if signal_count == 0:
        if missing_data_dates:
            logger.error("BACKTEST FAILED: No data available for trading days")
        else:
            logger.error("BACKTEST FAILED: No signals generated")
        return 1
    elif len(closed_trades) == 0:
        logger.warning("BACKTEST COMPLETED: No trades executed")
        return 2
    else:
        logger.info("BACKTEST COMPLETED SUCCESSFULLY")
        return 0

def main():
    parser = argparse.ArgumentParser(description='Trading Strategy Analysis')
    parser.add_argument('-r', '--recommend', action='store_true', help='Run recommendation mode')
    parser.add_argument('-b', '--backtest', action='store_true', help='Run backtest mode')
    args = parser.parse_args()
    if not (args.recommend or args.backtest):
        parser.error("Please specify either -r/--recommend or -b/--backtest")
    if args.recommend:
        run_recommendation()
    elif args.backtest:
        run_backtest()

if __name__ == "__main__":
    main()
